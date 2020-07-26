using System.IO;
using System.Text;
using UnityEngine;

/*
                            Json 파일쓰고 읽기에 필요한 Util함수들
 */

public class JsonManager
{
    public static void ObjectToJsonWithCreate(string createPath, string fileName, object obj)
    {
        using (FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create))
        {
            var json = ObjectToJson(obj);

            byte[] data = Encoding.UTF8.GetBytes(json);
            fileStream.Write(data, 0, data.Length);
            fileStream.Close();
        }
    }

    public static T LoadJson<T>(string loadPath, string fileName)
    {
        using (FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open))
        {
            byte[] data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);
            fileStream.Close();

            string jsonData = Encoding.UTF8.GetString(data);
            return JsonToObject<T>(jsonData);
        }
    }

    public static string ObjectToJson(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    public static T JsonToObject<T>(string json)
    {
        return JsonUtility.FromJson<T>(json);
    }
}
