using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;

namespace TheService.Extension.Formatter
{
    public class NewtonsoftJsonClientFormatter : IClientMessageFormatter
    {
        OperationDescription operation;
        public NewtonsoftJsonClientFormatter(OperationDescription operation)
        {
            this.operation = operation;
        }

        public object DeserializeReply(Message message, object[] parameters)
        {
            XmlDictionaryReader bodyReader = message.GetReaderAtBodyContents();
            JsonSerializer serializer = new JsonSerializer();
            bodyReader.ReadStartElement("Binary");
            byte[] body = bodyReader.ReadContentAsBase64();
            using (MemoryStream ms = new MemoryStream(body))
            {
                using (StreamReader sr = new StreamReader(ms))
                {
                    Type returnType = this.operation.Messages[1].Body.ReturnValue.Type;
                    return serializer.Deserialize(sr, returnType);
                }
            }
        }

        public Message SerializeRequest(MessageVersion messageVersion, object[] parameters)
        {
            byte[] body;
            JsonSerializer serializer = new JsonSerializer();
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8))
                {
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                        if (parameters.Length == 1)
                        {
                            // Single parameter, assuming bare
                            serializer.Serialize(sw, parameters[0]);
                        }
                        else
                        {
                            writer.WriteStartObject();
                            for (int i = 0; i < this.operation.Messages[0].Body.Parts.Count; i++)
                            {
                                writer.WritePropertyName(this.operation.Messages[0].Body.Parts[i].Name);
                                serializer.Serialize(writer, parameters[0]);
                            }

                            writer.WriteEndObject();
                        }

                        writer.Flush();
                        sw.Flush();
                        body = ms.ToArray();
                    }
                }
            }

            Message requestMessage = Message.CreateMessage(messageVersion, operation.Messages[0].Action, new RawBodyWriter(body));
            return requestMessage;
        }

    }
}