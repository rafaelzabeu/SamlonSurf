using System.Collections;
using System.IO;
using System.Xml.Serialization;

public class SimpleXMLSerializer
{
    static public T Deserialize<T>(string data)
    {
        T outerObject;
        XmlSerializer serializer = new XmlSerializer(typeof(T));

        using (StringReader reader = new StringReader(data))
            outerObject = (T) serializer.Deserialize(reader);

        return outerObject;
    }


    static public string Serialize<T>(T data)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));

        using (StringWriter sWriter = new StringWriter())
        {
            using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(sWriter))
            {
                serializer.Serialize(writer, data);
                return sWriter.ToString();
            }
        }
    }
}
