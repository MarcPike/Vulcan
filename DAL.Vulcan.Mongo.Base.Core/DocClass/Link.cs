using System;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using MongoDB.Bson;

namespace DAL.Vulcan.Mongo.Base.Core.DocClass
{
    public class Link
    {
        public ObjectId Id { get; private set; }
        public string TypeFullName { get; private set; }
        public string AssemblyQualifiedName { get; private set; }

        public Link(BaseDocument doc)
        {
            Id = doc.Id;
            TypeFullName = doc.GetType().FullName;
            AssemblyQualifiedName = doc.GetType().AssemblyQualifiedName;
        }

        public BaseDocument ToBaseDocument()
        {
            // Use some reflection to get the repository
            Type genericType = typeof(RepositoryBase<>);
            Type[] typeArgs = {Type.GetType(AssemblyQualifiedName)};
            Type repositoryType = genericType.MakeGenericType(typeArgs);

            // using a dynamic here makes this much easier
            dynamic repository = Activator.CreateInstance(repositoryType);

            // now any method we call is valid if it exists in our repository
            return repository.Find(Id);

            //// We can rely on reflection and get the FindByString method to locate our document
            //// Note, if you use overloading you are screwed, so I had to create this method
            //MethodInfo genericMethod = repositoryType.GetMethod("FindByString");

            //// not too sure about this syntax but it seems to work
            //var result = genericMethod.Invoke(repository, new object[] { Id.ToString() });

            //// return null or the BaseDocument value
            //return result as BaseDocument;
        }
    }
}