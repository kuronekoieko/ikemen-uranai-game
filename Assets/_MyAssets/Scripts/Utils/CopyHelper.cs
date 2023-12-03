using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// コピー用汎用関数
/// </summary>
public static class CopyHelper
{
    /// <summary>
    /// 対象のディープコピーを行う
    /// シリアライズ(Serializable 属性)されていないクラスではエラー
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="src"></param>
    /// <returns></returns>
    public static T DeepCopy<T>(this T src)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, src);
            stream.Position = 0;

            return (T)formatter.Deserialize(stream);
        }
    }
}
