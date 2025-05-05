using AbilitySystem;
using System.Collections.Generic;
using UnityEngine;
using Client;

[System.Serializable]
public class LocalMapGenerationConfig
{
    public int MaxLinkCount = 3;
    public int Radius = 2;
    [Header("Map Size")]
    public int MaxWidth = 10;
    public int MaxLength = 10;
    public GameObject MeshCreator;
    [Header("Bioms")]
    public RoomGOConfig[] Bioms;
    public RoomGOConfig GetBiomByIndex(int index)
    {
        index = Mathf.Clamp(index, 0, Bioms.Length);
        return Bioms[index];
    }
    public int[] GetRandomBiomsByBiomCount(int biomCount)
    {
        int[] biom = new int[Bioms.Length];
        for (int i = 0; i < biom.Length; i++) biom[i] = i;
        
        
        // Список для заполнения
        List<int> serviceList = new List<int>();

        // Создаем очередь из элементов biom
        Queue<int> biomQueue = new Queue<int>(biom);

        // Заполняем массив test
        for (int i = 0; i < biomCount; i++)
        {
            // Если очередь пуста, обновляем её с элементами biom
            if (biomQueue.Count == 0)
            {
                biomQueue = new Queue<int>(biom);
            }
            if (biomQueue.Count > 0)
            {
                ShuffleQueue(biomQueue);
            }
            // Извлекаем элемент из очереди
            int randomElement = biomQueue.Dequeue();
            serviceList.Add(randomElement);

            // Если очередь еще не пуста, перемешиваем элементы
            
        }
        // Преобразуем список в массив
        int[] result = serviceList.ToArray();
        
        return result;
    }
    
    
   private void ShuffleQueue(Queue<int> queue)
    {
        List<int> list = new List<int>(queue);
        System.Random rand = new System.Random();

        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rand.Next(0, i + 1);
            // Меняем местами элементы
            int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        // Обновляем очередь
        queue.Clear();
        foreach (var item in list)
        {
            queue.Enqueue(item);
        }
    }

}