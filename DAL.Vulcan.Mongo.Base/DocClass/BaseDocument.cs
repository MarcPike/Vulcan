using AspNetCore.Identity.MongoDB.Validators;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using DAL.Vulcan.Mongo.Base.Clone;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using JsonConvert = MongoDB.Bson.IO.JsonConvert;
using Newtonsoft.Json.Converters;

namespace DAL.Vulcan.Mongo.Base.DocClass
{

    public class BaseDocument: ObjectWithTags, ISupportInitialize, IEqualityComparer<BaseDocument>
    {
        public DocVersion Version { get; set; } = new DocVersion();
        public string CreatedByUserId { get; set; } = string.Empty;
        public string ModifiedByUserId { get; set; } = string.Empty;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime = DateTime.Now;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ModifiedDateTime = DateTime.Now;

        public List<string> SearchTags { get; set; } = new List<string>();

        [BsonExtraElements]
        public IDictionary<string, object> ExtraElements { get; set; }

        [BsonId]
        //[BsonIgnoreIfDefault]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        public List<Link> Links { get; set; } = new List<Link>();

        public List<ExternalDocumentList> ExternalDocuments { get; set; } = new List<ExternalDocumentList>();
        public ExternalDocumentList GetExternalDocumentList(string name)
        {
            return ExternalDocuments.FirstOrDefault(x => x.Name == name);
        }

        public void SaveExternalDocumentList(ExternalDocumentList list)
        {
            RemoveExternalDocumentList(list.Name);
            ExternalDocuments.Add(list);
        }

        public void RemoveExternalDocumentList(string name)
        {
            var existingList = ExternalDocuments.FirstOrDefault(x => x.Name == name);
            if (existingList != null) ExternalDocuments.Remove(existingList);
        }

        public virtual List<ValidationError> Validate()
        {
            return new List<ValidationError>();
        }

        public void CreateLinkOneWay(BaseDocument attachDoc)
        {
            if (Links.All(x => x.Id != attachDoc.Id))
            {
                Links.Add(new Link(attachDoc));
                SaveToDatabase();
            }
        }

        public void CreateLink(BaseDocument attachDoc)
        {
            if (Links.All(x => x.Id != attachDoc.Id))
                Links.Add(new Link(attachDoc));
            SaveToDatabase();
            attachDoc.CreateLinkOneWay(this);
        }

        public void RemoveLink(BaseDocument attachedDoc)
        {
            var existingLink = Links.SingleOrDefault(x => x.Id == attachedDoc.Id && x.TypeFullName == attachedDoc.GetType().FullName);
            if (existingLink != null)
            {
                Links.Remove(existingLink);
                SaveToDatabase();
            }
            attachedDoc.RemoveLinkOneWay(this);
        }

        public void RemoveLinkForTypeAndId(Type type, string id)
        {
            foreach (var link in Links.ToList())
            {
                if ((link.TypeFullName == type.FullName) && (link.Id.ToString() == id))
                {
                    Links.Remove(link);
                    SaveToDatabase();
                }
            }
        }


        public void RemoveLinkOneWay(BaseDocument attachedDoc)
        {
            var existingLink = Links.SingleOrDefault(x => x.Id == attachedDoc.Id && x.TypeFullName == attachedDoc.GetType().FullName);
            if (existingLink != null)
            {
                Links.Remove(existingLink);
            }
            SaveToDatabase();
        }

        public void RemoveAllLinks()
        {
            foreach (var link in Links.ToList())
            {
                var otherDoc = link.ToBaseDocument();
                if (otherDoc != null)
                {
                    RemoveLink(otherDoc);
                }
            }
        }

        public void RemoveAllLinksForType(Type type)
        {
            foreach (var link in Links.ToList())
            {
                if (link.TypeFullName == type.FullName)
                {
                    var otherDoc = link.ToBaseDocument();
                    if (otherDoc != null)
                    {
                        RemoveLink(otherDoc);
                    }
                }
            }
        }

        public List<Link> GetAllLinks()
        {
            return Links.ToList();
        }

        public List<Link> GetAllLinksForType(string typeFullName)
        {
            return Links.Where(x=>x.TypeFullName == typeFullName).ToList();
        }

        public List<Link> GetAllLinksForType(Type type)
        {
            return Links.Where(x => x.TypeFullName == type.FullName).ToList();
        }

        public virtual void SaveToDatabase()
        {
            Type genericType = typeof(RepositoryBase<>);
            var thisType = this.GetType().AssemblyQualifiedName;
            if (thisType == null) return;

            Type[] typeArgs = { Type.GetType(thisType) };
            Type repositoryType = genericType.MakeGenericType(typeArgs);

            // using a dynamic here makes this much easier
            object repository = Activator.CreateInstance(repositoryType);

            //// We can rely on reflection and get the FindByString method to locate our document
            //// Note, if you use overloading you are screwed, so I had to create this method
            MethodInfo genericMethod = repositoryType.GetMethod("Upsert");

            if (genericMethod == null) throw new Exception("No Upsert method found in repository");

            //// not too sure about this syntax but it seems to work
            var result = genericMethod.Invoke(repository, new object[] { this });
        }

        public virtual void Remove()
        {
            Type genericType = typeof(RepositoryBase<>);
            var thisType = this.GetType().AssemblyQualifiedName;
            if (thisType == null) return;

            Type[] typeArgs = { Type.GetType(thisType) };
            Type repositoryType = genericType.MakeGenericType(typeArgs);

            // using a dynamic here makes this much easier
            object repository = Activator.CreateInstance(repositoryType);

            //// We can rely on reflection and get the FindByString method to locate our document
            //// Note, if you use overloading you are screwed, so I had to create this method
            MethodInfo genericMethod = repositoryType.GetMethod("RemoveOne");

            if (genericMethod == null) throw new Exception("No RemoveOne method found in repository");

            //// not too sure about this syntax but it seems to work
            var result = genericMethod.Invoke(repository, new object[] { this });
        }

        /// <summary>
        /// Usually this is left blank
        /// </summary>
        public virtual void BeginInit()
        {
            /// <summary>
            /// Example:
            ///  object nameValue;
            ///if (!ExtraElements.TryGetValue("Name", out nameValue)) {
            ///    return;
            ///}
            ///
            ///var name = (string)nameValue;

            /// // remove the Name element so that it doesn't get persisted back to the database
            ///ExtraElements.Remove("Name");

            /// // assuming all names are "First Last"
            ///var nameParts = name.Split(' ');

            ///FirstName = nameParts[0];
            ///LastName = nameParts[1];
            /// </summary>
        }

        public virtual void EndInit()
        {
        }

        public bool Equals(BaseDocument x, BaseDocument y)
        {
            if ((x == null) || (y == null)) return false;
            return x.Id == y.Id;
        }

        public int GetHashCode(BaseDocument obj)
        {
            return obj.GetHashCode();
        }

        public BaseDocument DeepClone()
        {
            var doc = BsonDocument.Parse(this.ToJson());
            return BsonSerializer.Deserialize<BaseDocument>(doc);
        }
    }

   
}