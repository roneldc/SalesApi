namespace SalesApi.Helpers
{
    public static class FileHelper
    {
        public static bool IsValidCsvFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return false;
            }

            var extension = Path.GetExtension(file.FileName);
            Console.WriteLine(extension);
            return extension.ToLower() == ".csv";
        }
    }
}