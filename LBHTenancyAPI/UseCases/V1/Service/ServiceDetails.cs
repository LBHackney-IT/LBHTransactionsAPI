namespace LBHTenancyAPI.UseCases.V1.Service
{
    public class ServiceDetails
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Organisation { get; set; }
        public ServiceDetailVersion Version { get; set; }
    }
}