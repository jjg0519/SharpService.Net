using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;

namespace TheService.Extension.Formatter
{
    public class NewtonsoftJsonDispatchFormatter : IDispatchMessageFormatter
    {
        OperationDescription operation;
        public NewtonsoftJsonDispatchFormatter(OperationDescription operation, bool isRequest)
        {
            this.operation = operation;
        }

        public void DeserializeRequest(Message message, object[] parameters)
        {
            XmlDictionaryReader bodyReader = message.GetReaderAtBodyContents();
            JsonSerializer serializer = new JsonSerializer();
            bodyReader.ReadStartElement("Binary");
            byte[] body = bodyReader.ReadContentAsBase64();
            using (MemoryStream ms = new MemoryStream(body))
            {
                using (StreamReader sr = new StreamReader(ms))
                {
                    var _Json = (JObject)JsonConvert.DeserializeObject(sr.ReadToEnd());
                    var parts = operation.Messages[0].Body.Parts;
                    for (var i = 0; i < parts.Count; i++)
                    {
                        parameters[i] = JsonConvert.DeserializeObject(
                            _Json[operation.Messages[0].Body.Parts[i].Name].ToString(),
                            operation.Messages[0].Body.Parts[i].Type);
                    }
                }
            }

            //XmlDictionaryReader bodyReader = message.GetReaderAtBodyContents();
            //bodyReader.ReadStartElement("Binary");
            //byte[] rawBody = bodyReader.ReadContentAsBase64();
            //MemoryStream ms = new MemoryStream(rawBody);

            //StreamReader sr = new StreamReader(ms);
            //Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();

            //if (parameters.Length == 1)
            //{
            //    // single parameter, assuming bare
            //    parameters[0] = serializer.Deserialize(sr, operation.Messages[0].Body.Parts[0].Type);
            //}
            //else
            //{
            //    // multiple parameter, needs to be wrapped
            //    Newtonsoft.Json.JsonReader reader = new Newtonsoft.Json.JsonTextReader(sr);
            //    reader.Read();
            //    if (reader.TokenType != Newtonsoft.Json.JsonToken.StartObject)
            //    {
            //        throw new InvalidOperationException("Input needs to be wrapped in an object");
            //    }

            //    reader.Read();
            //    while (reader.TokenType == Newtonsoft.Json.JsonToken.PropertyName)
            //    {
            //        string parameterName = reader.Value as string;
            //        reader.Read();
            //        if (this.parameterNames.ContainsKey(parameterName))
            //        {
            //            int parameterIndex = this.parameterNames[parameterName];
            //            parameters[parameterIndex] = serializer.Deserialize(reader, this.operation.Messages[0].Body.Parts[parameterIndex].Type);
            //        }
            //        else
            //        {
            //            reader.Skip();
            //        }

            //        reader.Read();
            //    }

            // reader.Close();
            // }

            //sr.Close();
            //ms.Close();
        }

        public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result)
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
                        serializer.Serialize(writer, result);
                        sw.Flush();
                        body = ms.ToArray();
                    }
                }
            }
            Message replyMessage = Message.CreateMessage(messageVersion, operation.Messages[1].Action, new RawBodyWriter(body));
            return replyMessage;
        }
    }

}