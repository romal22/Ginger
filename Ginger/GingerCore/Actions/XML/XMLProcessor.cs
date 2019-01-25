#region License
/*
Copyright © 2014-2018 European Support Limited

Licensed under the Apache License, Version 2.0 (the "License")
you may not use this file except in compliance with the License.
You may obtain a copy of the License at 

http://www.apache.org/licenses/LICENSE-2.0 

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS, 
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
See the License for the specific language governing permissions and 
limitations under the License. 
*/
#endregion

using Amdocs.Ginger.Repository;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace GingerCore.Actions.XML
{
    public class XMLProcessor
    {
        //TODO: write Unit test

        public void ParseToReturnValues(string XML, Act act)
        {
            XmlDocument xmlReqDoc = new XmlDocument();
            xmlReqDoc.LoadXml(XML);            
            SetOutput(xmlReqDoc, act);
        }

        // Read the XML for each elem generate output with Path
        private void SetOutput(XmlDocument xmlDoc, Act act)
        {
            XmlReader xmlReader = XmlReader.Create(new System.IO.StringReader(xmlDoc.InnerXml));
            XmlReader subTreeXmlReader = null;
            string xmlElement = "";
            
            ArrayList xmlElementArrayList = new ArrayList();
            List<string> DeParams = new List<string>();
            foreach (ActReturnValue returnValue in act.ReturnValues)
            {
                //TODO: How the user will know? add check box?
                if (returnValue.Param.IndexOf("AllDescOf") == 0)
                    DeParams.Add(returnValue.Param.Trim().Substring(9).Trim());
            }

            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    xmlElement = xmlReader.Name;
                    if (xmlElementArrayList.Count <= xmlReader.Depth)
                        xmlElementArrayList.Add(xmlElement);
                    else
                        xmlElementArrayList[xmlReader.Depth] = xmlElement;
                    foreach (var p in DeParams)
                    {
                        if (p == xmlReader.Name)
                        {
                            subTreeXmlReader = xmlReader.ReadSubtree();
                            subTreeXmlReader.ReadToFollowing(p);
                            act.AddOrUpdateReturnParamActualWithPath("AllDescOf" + p, subTreeXmlReader.ReadInnerXml(), "/" + string.Join("/", xmlElementArrayList.ToArray().Take(xmlReader.Depth)));
                            subTreeXmlReader = null;
                        }
                    }
                }

                if (xmlReader.NodeType == XmlNodeType.Text)
                {
                    // soup req contains sub xml, so parse them 
                    if (xmlReader.Value.StartsWith("<?xml"))
                    {
                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.LoadXml(xmlReader.Value);
                        SetOutput(xmlDocument, act);
                    }
                    else
                    {
                        act.AddOrUpdateReturnParamActualWithPath(xmlElement, xmlReader.Value, "/" + string.Join("/", xmlElementArrayList.ToArray().Take(xmlReader.Depth)));
                    }
                }
            }
        }
    }
}
