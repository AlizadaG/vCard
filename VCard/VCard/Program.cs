using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using VCard.Models;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Olusturmak istediginiz vCard adet sayısını giriniz: ");
        try
        {
            int count = Convert.ToInt32(Console.ReadLine());
            using HttpClient client = new HttpClient();
            try
            {
                if (count > 2000 || count <= 0) {
                    Console.WriteLine("Girdiginiz sayi 0 ile 2000 arasinda olmak zorunda!!!");
                }
                else
                {
                    HttpResponseMessage response = await client.GetAsync($"https://randomuser.me/api/?results={count}");
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var resultData = JsonSerializer.Deserialize<Result>(responseBody, options);
                    string parentDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\.."));
                    string vcfPath = Path.Combine(parentDirectory, "vCards(vcf)"),
                        txtPath = Path.Combine(parentDirectory, "vCards(txt)");

                    foreach (var user in resultData.Results)
                    {
                        StringBuilder card = new StringBuilder();
                        card.Append($@"BEGIN:VCARD
                                VERSION:4.0
                                N:{user.Name.First};{user.Name.Last};;;
                                FN:{user.Name.First} {user.Name.Last}
                                TEL;TYPE=work,voice;VALUE=uri:tel:{user.Phone}
                                TEL;TYPE=home,voice;VALUE=uri:tel:{user.Phone}
                                ADR;TYPE=WORK:;;{user.Location.Country};;{user.Location.City}
                                EMAIL:{user.Email}
                                REV:20080424T195243Z
                                END:VCARD");

                        string fileNameVcf = $"{user.Name.First}-{user.Name.Last}.{user.Id.Value}.vcf";
                        string filePathVcf = Path.Combine(vcfPath, fileNameVcf);
                        File.WriteAllTextAsync(filePathVcf, card.ToString());

                        string fileNameTxt = $"{user.Name.First}-{user.Name.Last}.{user.Id.Value}.txt";
                        string filePathTxt = Path.Combine(txtPath, fileNameTxt);
                        File.WriteAllTextAsync(filePathTxt, card.ToString());

                        Console.WriteLine($"{user.Name.First} {user.Name.Last} isimli userin vCard dosyasinin yolu :{filePathVcf}");
                    }
                }                
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Hata mesaji: {e.Message}");
            }
        }
        catch(Exception message) {
            Console.WriteLine("Sayıyı doğru formatda dahil ediniz. Hata mesajı:" + message);
        }
    }
}
