using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace TheHeroesJourney
{
    public class FileDataHandler
    {
        private string dataDirPath = "";
        private string dataFileName = "";

        private bool useEncryption = false;
        private readonly string encryptionCodeWord = "quynh";
        private readonly string backupExtension = ".bak";

        public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
        {
            this.dataDirPath = dataDirPath;
            this.dataFileName = dataFileName;
            this.useEncryption = useEncryption;
        }

        public GameData Load(bool allowRestoreFromBackup = true)
        {
            //use Path.Combine to account for different OS's different path separators
            string fullPath = Path.Combine(dataDirPath, dataFileName);

            GameData loadedData = null;
            if(File.Exists(fullPath))
            {
                try
                {
                    //load the serialized data from the file
                    string dataToLoad = "";
                    using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }


                    //optionally dencrypt the data
                    if (useEncryption)
                    {
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    }

                    //deserialize the data from Json  bakc into the C# object
                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    //since Load() is being called recursively, need to account for the case where the rollback success, 
                    //but the data is still failing to load for some other reason,
                    //which without this may cause an infinite recursion loop
                    if (allowRestoreFromBackup)
                    {
                        FirebaseManager.LogEvent("Error_FailedToLoadDataFileAttemptingToRollback");
                        Debug.LogWarning($"Failed to load data file. Attempting to rollback \n{e}");
                        bool rollbackSuccess = AttemptRollback(fullPath);
                        if (rollbackSuccess)
                        {
                            //try to load again recursively
                            loadedData = Load(false);
                        }
                    }
                    else
                    {
                        FirebaseManager.LogEvent("Error_TryingToLoadDataAndBackupDidNotWork");
                        Debug.LogError($"Error occured when trying to load file at path: {fullPath} and backup did not work \n{e}");
                        if(UIManager.Instance != null && UIManager.Instance.showErrorPanel != null)
                            UIManager.Instance.ShowErrorPanel($"Error occured when trying to load data and backup did not work!");
                        else if (MainMenuManager.Instance != null && MainMenuManager.Instance.showErrorPanel != null)
                            MainMenuManager.Instance.ShowErrorPanel($"Error occured when trying to load data and backup did not work!");
                    }
                }
            }
            return loadedData;
        }

        public void Save(GameData data)
        {
            //use Path.Combine to account for different OS's different path separators
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            string backupFilePath = fullPath + backupExtension;
            try
            {
                //Create the directory the file will be written to if it doesn't already exitst
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                //serialize the C# game data object to Json
                string dataToStore = JsonUtility.ToJson(data, true);

                //optionally encrypt the data
                if(useEncryption)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }

                //write the serialized data to the file
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }

                //verify the newly saved file can be loaded successfully
                GameData verifiedGameData = Load();
                //if the data can be verified, back it up
                if(verifiedGameData != null)
                {
                    File.Copy(fullPath, backupFilePath, true);
                }
                //otherwise, something went wrong
                else
                {
                    FirebaseManager.LogEvent("Error_SaveFileCouldNotBeVerifiedAndBackUpCouldNotBeCreated");
                    throw new Exception("Save file could not be verified and back up could not be created.");
                }
            }
            catch (Exception e)
            {
                FirebaseManager.LogEvent("Error_SaveDataFailed");
                Debug.LogError($"Error occured when trying to save data to file: {fullPath} \n {e}" );
                if (UIManager.Instance != null && UIManager.Instance.showErrorPanel != null)
                    UIManager.Instance.ShowErrorPanel($"Error occured when trying to save data!");
                else if (MainMenuManager.Instance != null && MainMenuManager.Instance.showErrorPanel != null)
                    MainMenuManager.Instance.ShowErrorPanel($"Error occured when trying to save data!");
            }
        }

        public void Delete()
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            try
            {
                //ensure the file data exists at this path before deleting
                if(File.Exists(fullPath))
                {
                   File.Delete(fullPath);
                }
                else
                {
                    FirebaseManager.LogEvent("Error_TriedToDeletingSavedGameDataButDataWasNotFound");

                    Debug.LogWarning($"Tried to deleting saved game data, but data was not found at path: {fullPath}");
                }
            }
            catch(Exception e)
            {
                FirebaseManager.LogEvent("Error_DeleteSavedDataFailed");
                Debug.LogError($"Fail to delete saved game data at path: {fullPath}\n{e}");
                if (UIManager.Instance != null && UIManager.Instance.showErrorPanel != null)
                    UIManager.Instance.ShowErrorPanel($"Fail to delete saved game data!");
                else if(MainMenuManager.Instance !=null && MainMenuManager.Instance.showErrorPanel != null)
                    MainMenuManager.Instance.ShowErrorPanel($"Fail to delete saved game data!");
            }
        }


        //simple implementation of XOR encryption 
        private string EncryptDecrypt(string data) 
        {
            string modifiedData = "";
            for(int i = 0; i < data.Length; i++)
            {
                modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
            }
            return modifiedData;
        }

        private bool AttemptRollback(string fullPath)
        {
            bool success = false;

            string backupFilePath = fullPath + backupExtension;
            try
            {
                if(File.Exists(backupFilePath))
                {
                    File.Copy(backupFilePath, fullPath, true);
                    success = true;
                    Debug.LogWarning($"Had to roll back to backup file at: {backupFilePath}");
                    FirebaseManager.LogEvent("Error_RollBackToBackupFile");
                }
                //otherwise, don't yet have a backup file - not thing to rollback
                else
                {
                    FirebaseManager.LogEvent("Error_NoBackupFileExistsRoRollbackTo");
                    throw new Exception("Tried to rollback, but no backup file exists to rollback to.");
                }
            }
            catch (Exception e)
            {
                FirebaseManager.LogEvent("Error_RollBackToBackupFileFailed");

                Debug.LogError($"Error occured when trying to roll back to backup file at: {backupFilePath} \n {e}");
                if (UIManager.Instance != null && UIManager.Instance.showErrorPanel != null)
                    UIManager.Instance.ShowErrorPanel($"Error occured when trying to roll back to backup file!");
                else if (MainMenuManager.Instance != null && MainMenuManager.Instance.showErrorPanel != null)
                    MainMenuManager.Instance.ShowErrorPanel($"Error occured when trying to roll back to backup file!");
            }

            return success;
        }
    }
}
