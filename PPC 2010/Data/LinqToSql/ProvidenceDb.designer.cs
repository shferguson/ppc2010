﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.261
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PPC_2010.Data.LinqToSql
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="providence")]
	public partial class ProvidenceDbDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public ProvidenceDbDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["providenceConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public ProvidenceDbDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ProvidenceDbDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ProvidenceDbDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ProvidenceDbDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Sermon> Sermons
		{
			get
			{
				return this.GetTable<Sermon>();
			}
		}
		
		public System.Data.Linq.Table<Article> Articles
		{
			get
			{
				return this.GetTable<Article>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="ppc2010.view_Sermons")]
	public partial class Sermon
	{
		
		private int _Id = default(int);
		
		private string _UmbracoTitle;
		
		private System.Nullable<System.DateTime> _RecordingDate;
		
		private string _Title;
		
		private string _SpeakerName;
		
		private string _RecordingSession;
		
		private string _SermonSeries;
		
		private string _Book;
		
		private System.Nullable<int> _StartChapter;
		
		private System.Nullable<int> _StartVerse;
		
		private System.Nullable<int> _EndChapter;
		
		private System.Nullable<int> _EndVerse;
		
		private string _ScriptureReferenceText;
		
		private string _AudioFile;
		
		public Sermon()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="Int NOT NULL", UpdateCheck=UpdateCheck.Never)]
		public override int Id
		{
			get
			{
				return this._Id;
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UmbracoTitle", DbType="NVarChar(255)")]
		public string UmbracoTitle
		{
			get
			{
				return this._UmbracoTitle;
			}
			set
			{
				if ((this._UmbracoTitle != value))
				{
					this._UmbracoTitle = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RecordingDate", DbType="DateTime")]
		public override System.Nullable<System.DateTime> RecordingDate
		{
			get
			{
				return this._RecordingDate;
			}
			set
			{
				if ((this._RecordingDate != value))
				{
					this._RecordingDate = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Title", DbType="NVarChar(500)")]
		public override string Title
		{
			get
			{
				return this._Title;
			}
			set
			{
				if ((this._Title != value))
				{
					this._Title = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SpeakerName", DbType="NVarChar(2500)")]
		public override string SpeakerName
		{
			get
			{
				return this._SpeakerName;
			}
			set
			{
				if ((this._SpeakerName != value))
				{
					this._SpeakerName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RecordingSession", DbType="NVarChar(2500)")]
		public override string RecordingSession
		{
			get
			{
				return this._RecordingSession;
			}
			set
			{
				if ((this._RecordingSession != value))
				{
					this._RecordingSession = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SermonSeries", DbType="NVarChar(2500)")]
		public override string SermonSeries
		{
			get
			{
				return this._SermonSeries;
			}
			set
			{
				if ((this._SermonSeries != value))
				{
					this._SermonSeries = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Book", DbType="NVarChar(2500)")]
		public override string Book
		{
			get
			{
				return this._Book;
			}
			set
			{
				if ((this._Book != value))
				{
					this._Book = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StartChapter", DbType="Int")]
		public override System.Nullable<int> StartChapter
		{
			get
			{
				return this._StartChapter;
			}
			set
			{
				if ((this._StartChapter != value))
				{
					this._StartChapter = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StartVerse", DbType="Int")]
		public override System.Nullable<int> StartVerse
		{
			get
			{
				return this._StartVerse;
			}
			set
			{
				if ((this._StartVerse != value))
				{
					this._StartVerse = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EndChapter", DbType="Int")]
		public override System.Nullable<int> EndChapter
		{
			get
			{
				return this._EndChapter;
			}
			set
			{
				if ((this._EndChapter != value))
				{
					this._EndChapter = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EndVerse", DbType="Int")]
		public override System.Nullable<int> EndVerse
		{
			get
			{
				return this._EndVerse;
			}
			set
			{
				if ((this._EndVerse != value))
				{
					this._EndVerse = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ScriptureReferenceText", DbType="NVarChar(500) NOT NULL")]
		public override string ScriptureReferenceText
		{
			get
			{
				return this._ScriptureReferenceText;
			}
			set
			{
				if ((this._ScriptureReferenceText != value))
				{
					this._ScriptureReferenceText = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AudioFile", DbType="NVarChar(500)")]
		public string AudioFile
		{
			get
			{
				return this._AudioFile;
			}
			set
			{
				if ((this._AudioFile != value))
				{
					this._AudioFile = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="ppc2010.view_Articles")]
	public partial class Article
	{
		
		private int _Id;
		
		private string _UmbracoTitle;
		
		private string _Title;
		
		private System.Nullable<System.DateTime> _Date;
		
		private string _Text;
		
		private string _ScriptureReference;
		
		public Article()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="Int NOT NULL")]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this._Id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UmbracoTitle", DbType="NVarChar(255)")]
		public string UmbracoTitle
		{
			get
			{
				return this._UmbracoTitle;
			}
			set
			{
				if ((this._UmbracoTitle != value))
				{
					this._UmbracoTitle = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Title", DbType="NVarChar(500)")]
		public string Title
		{
			get
			{
				return this._Title;
			}
			set
			{
				if ((this._Title != value))
				{
					this._Title = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Date", DbType="DateTime")]
		public System.Nullable<System.DateTime> Date
		{
			get
			{
				return this._Date;
			}
			set
			{
				if ((this._Date != value))
				{
					this._Date = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Text", DbType="NText", UpdateCheck=UpdateCheck.Never)]
		public string Text
		{
			get
			{
				return this._Text;
			}
			set
			{
				if ((this._Text != value))
				{
					this._Text = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ScriptureReference", DbType="NText", UpdateCheck=UpdateCheck.Never)]
		public string ScriptureReference
		{
			get
			{
				return this._ScriptureReference;
			}
			set
			{
				if ((this._ScriptureReference != value))
				{
					this._ScriptureReference = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
