using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services.Protocols;

namespace asmxTraceExtensionLog4Net
{
    /// <summary>
    /// 在所有的SOAP接口上面增加这个属性就可以了 [TraceLog]
    ///  Define a SOAP Extension that traces the SOAP request and SOAP
    /// response for the XML Web service method the SOAP extension is
    /// applied to. 
    /// </summary>
    public class SOAPTraceLog : SoapExtension
    {
        log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        Stream oldStream;
        Stream newStream;
        string filename;
        Type WebServiceType;
        // Save the Stream representing the SOAP request or SOAP response into
        // a local memory buffer.
        public override Stream ChainStream(Stream stream)
        {
            oldStream = stream;
            newStream = new MemoryStream();
            return newStream;
        }

        // When the SOAP extension is accessed for the first time, the XML Web
        // service method it is applied to is accessed to store the file
        // name passed in, using the corresponding SoapExtensionAttribute.	
        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            //return ((TraceLogAttribute)attribute).Filename;
            //return null;
            return AppDomain.CurrentDomain.BaseDirectory + "\\log\\soap.log";
        }

        // The SOAP extension was configured to run using a configuration file
        // instead of an attribute applied to a specific XML Web service
        // method.
        public override object GetInitializer(Type WebServiceType)
        {
            // Return a file name to log the trace information to, based on the
            // type.
            this.WebServiceType = WebServiceType;
            return AppDomain.CurrentDomain.BaseDirectory + "log\\soap.log";
            //return null;
        }

        // Receive the file name stored by GetInitializer and store it in a
        // member variable for this specific instance.
        public override void Initialize(object initializer)
        {
            filename = (string)initializer;
        }

        //  If the SoapMessageStage is such that the SoapRequest or
        //  SoapResponse is still in the SOAP format to be sent or received,
        //  save it out to a file.
        public override void ProcessMessage(SoapMessage message)
        {
            switch (message.Stage)
            {
                case SoapMessageStage.BeforeSerialize:
                    break;
                case SoapMessageStage.AfterSerialize:
                    WriteOutput(message);
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    WriteInput(message);
                    break;
                case SoapMessageStage.AfterDeserialize:
                    break;
            }
        }

        public void WriteOutput(SoapMessage message)
        {
            newStream.Position = 0;

            TextReader reader = new StreamReader(newStream);
            var s = reader.ReadToEnd();
            //log.Debug("请求地址为："+ message.Url + " 请求Action：" + message.Action +  " 出参为:" + s);
            log.Debug("出参为:" + s);

            newStream.Position = 0;
            Copy(newStream, oldStream);
        }

        public void WriteInput(SoapMessage message)
        {

            Copy(oldStream, newStream);

            newStream.Position = 0;

            TextReader reader = new StreamReader(newStream);
            var s = reader.ReadToEnd();
            log.Debug("请求地址为：" + message.Url);
            log.Debug("请求Action：" + message.Action);
            log.Debug("入参为:" + s);

            newStream.Position = 0;
        }

        void Copy(Stream from, Stream to)
        {
            TextReader reader = new StreamReader(from);
            TextWriter writer = new StreamWriter(to);
            writer.WriteLine(reader.ReadToEnd());
            writer.Flush();
        }
    }

    // Create a SoapExtensionAttribute for the SOAP Extension that can be
    // applied to an XML Web service method.
    [AttributeUsage(AttributeTargets.Method)]
    public class TraceLogAttribute : SoapExtensionAttribute
    {

        private int priority;

        public override Type ExtensionType
        {
            get { return typeof(SOAPTraceLog); }
        }

        public override int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

    }
}