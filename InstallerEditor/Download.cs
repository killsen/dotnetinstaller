using System;
using System.Xml;
using System.ComponentModel;

namespace InstallerEditor
{
	/// <summary>
	/// Summary description for Download.
	/// </summary>
	public class Download : IXmlClass
	{
		public Download():this("APP_TEMP_DOWNLOADPATH")
		{
		}
		public Download(string p_DownloadPath)
		{
			m_componentname = "New Component";
			m_sourceurl = "http://www.yourwebsite.com/SetupX/Setup.exe";
			m_destinationpath = "#TEMPPATH\\" + p_DownloadPath;
			m_destinationfilename = "";
		}

		private string m_componentname;
		[Description("The description of the file to download. (REQUIRED)")]
		public string componentname
		{
			get{return m_componentname;}
			set{m_componentname = value;OnComponentNameChanged(EventArgs.Empty);}
		}

		private string m_sourceurl;
		[Description("The complete source path of the file to download. For example 'http://www.yourwebsite.com/SetupX/Setup.exe' . Must be URL with http:// or ftp:// protocol (REQUIRED)")]
		public string sourceurl
		{
			get{return m_sourceurl;}
			set{m_sourceurl = value;}
		}

		private string m_destinationpath;
		[Description("The complete destination path where the application copy the sourceurl file. Is recommended to use the TEMP path for destination like this: '#TEMPPATH\\APPLICATION_NAME' . Can contains path constant (see Help->Path Constant). (REQUIRED)")]
		public string destinationpath
		{
			get{return m_destinationpath;}
			set{m_destinationpath = value;}
		}

		private string m_destinationfilename;
		[Description("New name of the downloaded file. Leave this value empty to use the same filename of the original filename. (OPTIONAL)")]
		public string destinationfilename
		{
			get{return m_destinationfilename;}
			set{m_destinationfilename = value;}
		}


		#region IXmlClass Members

		public void ToXml(XmlWriter p_Writer)
		{
			p_Writer.WriteStartElement("download");

			OnXmlWriteTagDownload(new XmlWriterEventArgs(p_Writer));

			p_Writer.WriteEndElement();
		}

		public void FromXml(XmlElement p_Element)
		{
			OnXmlReadTagDownload(new XmlElementEventArgs(p_Element));
		}
		#endregion


		protected virtual void OnXmlWriteTagDownload(XmlWriterEventArgs e)
		{
			e.XmlWriter.WriteAttributeString("componentname",m_componentname);
			e.XmlWriter.WriteAttributeString("sourceurl",m_sourceurl);
			e.XmlWriter.WriteAttributeString("destinationpath",m_destinationpath);
			e.XmlWriter.WriteAttributeString("destinationfilename",m_destinationfilename);
		}
		protected virtual void OnXmlReadTagDownload(XmlElementEventArgs e)
		{
			if (e.XmlElement.Attributes["componentname"] != null)
				m_componentname = e.XmlElement.Attributes["componentname"].InnerText;

			if (e.XmlElement.Attributes["destinationfilename"] != null)
				m_destinationfilename = e.XmlElement.Attributes["destinationfilename"].InnerText;

			if (e.XmlElement.Attributes["destinationpath"] != null)
				m_destinationpath = e.XmlElement.Attributes["destinationpath"].InnerText;

			if (e.XmlElement.Attributes["sourceurl"] != null)
				m_sourceurl = e.XmlElement.Attributes["sourceurl"].InnerText;
		}

		protected virtual void OnComponentNameChanged(EventArgs e)
		{
			if (ComponentNameChanged!=null)
				ComponentNameChanged(this, e);
		}

		public event EventHandler ComponentNameChanged;
	}
}
