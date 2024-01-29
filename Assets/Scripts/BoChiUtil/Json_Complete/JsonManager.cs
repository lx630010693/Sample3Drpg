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
            //�����ⲿȥnew����
        }

        public void SaveData(object data, string fileName, E_JsonType type = E_JsonType.Newton)//����������ѡ��json�洢��ʽ��Ĭ����LitJson
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
            //���ж����޴洢����
            string path = Application.persistentDataPath + "/"+ fileName + ".json";
            if (!File.Exists(path))
            {
                //���û�д洢���ļ����Ǿ�Ԥ���ļ�����Ѱ�ң�streamingAssets��
                path = Application.streamingAssetsPath + "/" + fileName + ".json";
            }
            if (!File.Exists(path))
            {
                //��û�оͷ���Ĭ��ֵ
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
