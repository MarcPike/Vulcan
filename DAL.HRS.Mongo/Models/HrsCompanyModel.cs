namespace DAL.HRS.Mongo.Models
{
    /*
    public class EntityModel
    {
        public string Name { get; set; }
        public List<LocationRef> Locations { get; set; }

        public List<EntityProperties> Properties { get; set; } = new List<EntityProperties>();

        public HrsCompanyModel() { }

        public HrsCompanyModel(HrsCompany company)
        {
            Name = company.Name;
            Locations = company.Locations;

            Properties = new RepositoryBase<HrsCompanyProperties>().AsQueryable()
                .Where(x => x.Company.Id == company.Id.ToString()).ToList();
        }

    }*/
}
