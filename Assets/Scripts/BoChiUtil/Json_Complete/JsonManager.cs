using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using Newtonsoft.Json;

namespace BoChi
{
    public enum E_JsonType
    {
        JsonUtility,
        LitJson,
        Newton,
    }
   
    public class JsonManager
    {
        private static JsonManager instence = new JsonManager();

        public static JsonManager Instance
        {
            get
            {
                return instence;

            }
        }

        private JsonManager()
        {
            //避免外部去new该类
        }

        public void SaveData(object data, string fileName, E_JsonType type = E_JsonType.Newton)//第三个参数选择json存储方式，默认用LitJson
        {

            string path = Application.persistentDataPath + "/"  + fileName + ".json";
            Debug.Log(path);
          
            string jsonStr = "";
            switch (type)
            {
                case E_JsonType.JsonUtility:
                    jsonStr = JsonUtility.ToJson(data);
                    break;
                case E_JsonType.LitJson:
                    jsonStr = JsonMapper.ToJson(data);
                    break;
                case E_JsonType.Newton:
                    jsonStr = JsonConvert.SerializeObject(data, Formatting.Indented);
                    break;
            }
            File.WriteAllText(path, jsonStr);
        }

        public T LoadData<T>(string fileName,  E_JsonType type = E_JsonType.Newton) where T : new()
        {
            //先判断有无存储数据
            string path = Application.persistentDataPath + "/"+ fileName + ".json";
            if (!File.Exists(path))
            {
                //如果没有存储的文件，那就预设文件夹中寻找（streamingAssets）
                path = Application.streamingAssetsPath + "/" + fileName + ".json";
            }
            if (!File.Exists(path))
            {
                //都没有就返回默认值
                return new T();
            }
            string jsonStr = File.ReadAllText(path);
            T data = default(T);
            switch (type)
            {
                case E_JsonType.JsonUtility:
                    data = JsonUtility.FromJson<T>(jsonStr);
                    break;
                case E_JsonType.LitJson:
                    data = JsonMapper.ToObject<T>(jsonStr);
                    break;
                case E_JsonType.Newton:
                    data = JsonConvert.DeserializeObject<T>(jsonStr);
                    break;
            }
            return data;
        }
    }
}
