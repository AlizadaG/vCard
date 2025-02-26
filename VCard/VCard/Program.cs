﻿using System;
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
                //Api request zamani hata vermemesi icin istek sayisi 2000 olarak limitlendi ve eksi sayı girmesi önlendi
                if (count > 2000 || count <= 0) {
                    Console.WriteLine("Girdiğiniz sayı 0-dan büyük olmak zorunda ve maksiumum 2000 adet kişi ala bilirisiniz!!!");
                }
                else
                {
                    //Kullanicinin girdiği sayı dahilinde ulusal kimlikleri (us,fr,tr) olak filtrelenerek istek gönderildi
                    HttpResponseMessage response = await client.GetAsync($"https://randomuser.me/api/?nat=us,fr,tr&results={count}");
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    //API den alınan data deserialize edildi
                    var resultData = JsonSerializer.Deserialize<Result>(responseBody, options);

                    //Dosyayi kaydetmek için belirtilmiş dosyalarin yollari bulundu
                    string parentDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\.."));
                    string vcfPath = Path.Combine(parentDirectory, "vCards(vcf)"),
                        txtPath = Path.Combine(parentDirectory, "vCards(txt)");

                    foreach (var user in resultData.Results)
                    {
                        // Her bir userin vCardını oluşturmak için metoda apiden alinan degerler ve dosya yollari gonderildi
                        CreateCard(user, vcfPath, txtPath);
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
    /// <summary>
    /// vCard oluşturmak için 
    /// </summary>
    /// <param name="user">User datası</param>
    /// <param name="vcfPath">vcf dosyaların kaydetmek için yol</param>
    /// <param name="txtPath">txt dosyaların kaydetmek için yol</param>
    public static void CreateCard(vCard user, string vcfPath, string txtPath)
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

        string fileName = $"{user.Name.First}-{user.Name.Last}.{user.Id.Value}";

        string filePathVcf = Path.Combine(vcfPath, $"{fileName}.vcf");
        File.WriteAllTextAsync(filePathVcf, card.ToString());

        string filePathTxt = Path.Combine(txtPath, $"{fileName}.txt");
        File.WriteAllTextAsync(filePathTxt, card.ToString());

        Console.WriteLine($"{user.Name.First} {user.Name.Last} isimli userin vCard dosyasinin yolu : {filePathVcf}");
    }
}
