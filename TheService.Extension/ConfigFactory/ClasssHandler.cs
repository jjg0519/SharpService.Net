﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;
using TheService.Common;

namespace TheService.Extension.ConfigFactory
{
    public class ClasssHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            List<ClassElement> listClasss = new List<ClassElement>();
            XmlDocument doc = ConfigHelper.CreateXmlDoc(section.InnerXml);
            foreach (XmlNode classNode in doc.FirstChild.ChildNodes)
            {
                string errorMessage = string.Empty;
                ClassElement classElement = new ClassElement();
                var classProperties = classElement.GetType().GetProperties();
                foreach (var classPropertie in classProperties)
                {
                    var attr = classNode.Attributes[classPropertie.Name.ToLower()];
                    if (attr != null)
                    {
                        classPropertie.SetValue(classElement, Convert.ChangeType(attr.Value, classPropertie.PropertyType), null);
                    }
                }
                if (!ValidateHelper.ValidateEntity(classElement, out errorMessage))
                {
                    throw new Exception(errorMessage);
                }
                listClasss.Add(classElement);
            }
            return listClasss;
        }
    }
}