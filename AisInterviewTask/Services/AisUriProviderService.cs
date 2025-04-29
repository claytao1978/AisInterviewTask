using AisUriProviderApi;

namespace AisInterviewTask.Services
{
   public class AisUriProviderService
    {             
       private readonly AisUriProvider _uriProvider;
       
        public AisUriProviderService()
        {
            _uriProvider = new AisUriProvider();
        }

        public IEnumerable<Uri> FetchUris()
        {
            try
            {
                var fileUris = _uriProvider.Get();
                return fileUris;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured trying to fetch URIs: {ex.Message}");
                return [];
            }           
        }    
    }
}
