using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

public static class SaveModule
{
    public static readonly string VERSION = Application.version;
    private static readonly Dictionary<Type, IDatable> _registeredData = new Dictionary<Type, IDatable>();
    private static string SaveDirectory => Path.Combine(Application.persistentDataPath, "Saves");

    /// <summary>
    /// Initializes the SaveModule by registering all IDatable instances and loading their data.
    /// </summary>
    public static void Initialize()
    {
        Debug.Log("Start finding all data to register");

        // Find all IDatable
        var datableTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IDatable).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        foreach (var type in datableTypes)
        {
            // Create instances and add to save
            try
            {
                if (Activator.CreateInstance(type) is IDatable datableInstance)
                {
                    RegisterData(datableInstance);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        Debug.Log("All data registered successfully");

        Debug.Log("Start loading data");

        LoadAllData();
    }

    /// <summary>
    /// Saves all registered data instances to their respective files.
    /// </summary>
    public static void SaveAllData()
    {
        foreach (var type in _registeredData.Keys)
        {
            Debug.Log($"Attempting to save data of type {type.Name}...");

            try
            {
                var data = _registeredData[type];
                string savePath = Path.Combine(SaveDirectory, $"{type.Name}.data");

                if (!Directory.Exists(SaveDirectory))
                {
                    Directory.CreateDirectory(SaveDirectory);
                }

                if (data is IIgnorable ignorable)
                    if (ignorable.IsAllSaveIgnored) continue;

                data.ProcessUpdataData();

                using (FileStream stream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, data);
                }

                Debug.Log($"Data of type {type.Name} saved successfully to {savePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error while saving data of type {type.Name}: {ex.Message}");
            }
        }

        Debug.Log("All data saved successfully");
    }

    /// <summary>
    /// Loads all registered data instances from their respective files.
    /// </summary>
    public static void LoadAllData()
    {
        // ������ ��������� ������ ������ ��� ����������� ��������
        var keys = _registeredData.Keys.ToList();

        const string VERSION_KEY = "GameDataVersion";

        string savedVersion = PlayerPrefs.GetString(VERSION_KEY, "0.0.1");

        if (savedVersion != VERSION) // CURRENT_VERSION - ваша текущая версия данных
        {
            Debug.Log($"Version mismatch! Saved: {savedVersion}, Current: {VERSION}. Resetting data...");
            PlayerPrefs.SetString(VERSION_KEY, VERSION); // Обновляем версию
            PlayerPrefs.Save();
            return;
        }

        foreach (var type in keys)
        {
            Debug.Log($"Attempting to load data of type {type.Name}...");

            try
            {
                string savePath = Path.Combine(SaveDirectory, $"{type.Name}.data");

                if (type is IIgnorable ignorable)
                    if (ignorable.IsAllLoadIgnored) continue;

                if (!Directory.Exists(SaveDirectory))
                {
                    Directory.CreateDirectory(SaveDirectory);
                }

                if (!File.Exists(savePath))
                {
                    Debug.LogWarning($"Save file for {type.Name} does not exist. Creating new instance.");

                    if (Activator.CreateInstance(type) is IDatable newData)
                    {
                        _registeredData[type] = newData;

                        using (FileStream stream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                        {
                            BinaryFormatter formatter = new BinaryFormatter();
                            formatter.Serialize(stream, newData);
                        }
                    }
                    continue;
                }

                using (FileStream stream = new FileStream(savePath, FileMode.Open, FileAccess.Read))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    var loadedData = (IDatable)formatter.Deserialize(stream);
                    _registeredData[type] = loadedData;
                }

                Debug.Log($"Data of type {type.Name} loaded successfully from {savePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error while loading data of type {type.Name}: {ex.Message}");
            }
        }

        Debug.Log("All data loaded successfully");
    }
    /// <summary>
    /// Special method for data operation
    /// </summary>
    private static T EnsureData<T>(Func<T> creationLogic, string errorMessage, string successMessage = null) where T : IDatable
    {
        var type = typeof(T);

        if (_registeredData.TryGetValue(type, out var data))
        {
            return (T)data;
        }

        try
        {
            // ������� ������
            var newData = creationLogic();
            RegisterData(newData);
            Debug.Log(successMessage ?? $"������ ���� {type.Name} ������� ������� � ����������������.");
            return newData;
        }
        catch (Exception ex)
        {
            Debug.LogError($"{errorMessage}: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Get data of type T, if can't get, create them and register 
    /// </summary>
    public static T GetData<T>() where T : IDatable, new()
    {
        return EnsureData(
            creationLogic: () => new T(),
            errorMessage: $"�� ������� ������� ������ ���� {typeof(T).Name}",
            successMessage: $"������ ���� {typeof(T).Name} ������� ������� � ���������."
        );
    }

    /// <summary>
    /// Save data of type T
    /// </summary>
    public static void SaveSingleData<T>() where T : IDatable
    {
        EnsureData<T>(
            creationLogic: () => throw new Exception($"������ ���� {typeof(T).Name} ����������� ��� ����������."),
            errorMessage: $"������ ��� ���������� ������ ���� {typeof(T).Name}"
        );

        try
        {
            string savePath = GetSavePath<T>();
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }

            var data = _registeredData[typeof(T)];

            data.ProcessUpdataData();

            using (FileStream stream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, data);
            }

            Debug.Log($"Data type of {typeof(T).Name} savved success.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"������ ��� ���������� ������ ���� {typeof(T).Name}: {ex.Message}");
        }
    }

    public static void DisposeSingleData<T>() where T : IDatable
    {
        try
        {
            string savePath = GetSavePath<T>();
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }

            var data = _registeredData[typeof(T)];

            data.Dispose();

            using (FileStream stream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, data);
            }

            Debug.Log($"Data type of {typeof(T).Name} savved success.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"������ ��� ���������� ������ ���� {typeof(T).Name}: {ex.Message}");
        }
    }

    /// <summary>
    /// Load data type of T 
    /// </summary>
    public static void LoadSingleData<T>() where T : IDatable, new()
    {
        Debug.Log($"Start single load {typeof(T)}");

        EnsureData<T>(
            creationLogic: () => new T(),
            errorMessage: $"������ ��� �������� ������ ���� {typeof(T).Name}"
        );

        try
        {
            string savePath = GetSavePath<T>();
            if (!File.Exists(savePath))
            {
                Debug.LogWarning($"���� ���������� ��� ������ ���� {typeof(T).Name} �����������. ��������� ����� ������.");
                SaveSingleData<T>();
                return;
            }

            using (FileStream stream = new FileStream(savePath, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                var loadedData = (T)formatter.Deserialize(stream);
                _registeredData[typeof(T)] = loadedData;
            }

            Debug.Log($"������ ���� {typeof(T).Name} ������� ���������.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"������ ��� �������� ������ ���� {typeof(T).Name}: {ex.Message}");
        }
    }

    /// <summary>
    /// Clear data type of T
    /// </summary>
    public static void DeleteData<T>() where T : IDatable
    {
        try
        {
            string savePath = GetSavePath<T>();
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
                Debug.Log($"���� ������ ���� {typeof(T).Name} ������� �����.");
            }

            if (_registeredData.ContainsKey(typeof(T)))
            {
                _registeredData.Remove(typeof(T));
                Debug.Log($"������ ���� {typeof(T).Name} ������� ������� �� ������.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"������ ��� �������� ������ ���� {typeof(T).Name}: {ex.Message}");
        }
    }

    /// <summary>
    /// Register new data in dictionary
    /// </summary>
    public static void RegisterData(IDatable data)
    {
        var type = data.GetType();
        if (!_registeredData.ContainsKey(type))
        {
            _registeredData[type] = data;

            Debug.Log($"[<color=cyan>{data.DATA_ID}</color>] data is registered successfully");
        }
    }

    /// <summary>
    /// Clears all data by deleting existing save files and creating fresh instances for all IDatable types.
    /// </summary>
    public static void ResetAllData()
    {
        Debug.Log("Starting reset of all data...");

        // ����� ��� ����, ������� ��������� IDatable
        var datableTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IDatable).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        foreach (var type in datableTypes)
        {
            Debug.Log($"Resetting data of type {type.Name}...");

            // ������� ���� ����������, ���� �� ����������
            string savePath = Path.Combine(SaveDirectory, $"{type.Name}.data");
            Debug.Log($"Trying to find data at {savePath}");
            if (File.Exists(savePath))
            {
                try
                {
                    File.Delete(savePath);
                    Debug.Log($"Deleted save file for {type.Name} at {savePath}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to delete save file for {type.Name}: {ex.Message}");
                    continue;
                }
            }
            else
            {
                Debug.Log($"No file at designated path");
            }

            // ������� ����� ��������� ������ � ����������������
            try
            {
                if (Activator.CreateInstance(type) is IDatable newData)
                {
                    if (!_registeredData.ContainsKey(type))
                    {
                        _registeredData[type] = newData;
                    }
                    else
                    {
                        _registeredData[type] = newData; // ������������ ������, ���� ��� ��� ����
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to reset data of type {type.Name}: {ex.Message}");
            }
        }

        SaveAllData();

        Debug.Log("All data reset successfully.");
    }

    /// <summary>
    /// Get path of data 
    /// </summary>
    private static string GetSavePath<T>() where T : IDatable
    {
        return Path.Combine(SaveDirectory, $"{typeof(T).Name}.data");
    }
}

public interface IDatable
{
    //Unique data id
    public string DATA_ID { get; }
    // Invoke when save is initialized, update needed data to save
    void ProcessUpdataData();
    void Dispose();
}

public interface IIgnorable
{
    public bool IsAllSaveIgnored { get; }
    public bool IsAllLoadIgnored { get; }
}