using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;


namespace PVT.Money.Business
{
    public static class CommissionAccountSerializer
    {
        public static void SerializeBinary(CommissionAccount account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account), ExceptionMessages.CASerializerNullAccount());
            BinaryFormatter bFormatter = new BinaryFormatter();
            using (FileStream stream = File.Create(account + ".txt"))
            {
                bFormatter.Serialize(stream, account);
            }                
        }

        public static CommissionAccount DeserializeBinary(string filePath)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException(ExceptionMessages.CASerializerFailPath());
            CommissionAccount buffer;
            BinaryFormatter bFormatter = new BinaryFormatter();
            using (FileStream stream = File.Open(filePath, FileMode.Open))
            {
                buffer = (CommissionAccount)bFormatter.Deserialize(stream);
            }
            return buffer;
        }

        public static void SerializeXML(CommissionAccount account, string filePath)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account), ExceptionMessages.CASerializerNullAccount());
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CommissionAccount));
            using (FileStream stream = File.Create(filePath))
            {
                xmlSerializer.Serialize(stream, account);
            }
        }

        public static void SerializeXML(CommissionAccount account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account), ExceptionMessages.CASerializerNullAccount());
            SerializeXML(account, account.AccountID + ".xml");
        }

        public static CommissionAccount DeserializeXML(string filePath)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException(ExceptionMessages.CASerializerFailPath());
            CommissionAccount buffer;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CommissionAccount));
            using (FileStream stream = File.Open(filePath, FileMode.Open))
            {
                buffer = (CommissionAccount)xmlSerializer.Deserialize(stream);
            }
            return buffer;
        }
    }
}
