using System;

namespace odev2{
	class KelimeAgaci{
		string Anlam = "";
		KelimeAgaci[] Dugum = new KelimeAgaci[26];
		
		public void KelimeEkle(string kelime, string anlam){
			KelimeAgaci[] Kok = new KelimeAgaci[26];
			Kok = Dugum;
			for(int i = 0; i < kelime.Length; i++){
				if (Dugum[kelime[i]-'a'] == null){
					if(i < kelime.Length){
						KelimeAgaci YeniDugum = new KelimeAgaci(); /* yeni dugum olustur */
						Dugum[kelime[i]-'a'] = YeniDugum;          /* ilgili karaktere karsilik yeni bir nesne referansi olustur */
						if(i == kelime.Length - 1){                /* son karakter ise */
							if(Dugum[kelime[i]-'a'].Anlam == ""){  /* ilk eklenen anlam ise */     
								Dugum[kelime[i]-'a'].Anlam += anlam; /* son karakterin nesne referansina anlami ekle */
							}
							else{ 
								Dugum[kelime[i]-'a'].Anlam += ";" + anlam;/* son karakterin nesne referansina diger anlami ekle */
							}
						}
						Dugum = Dugum[kelime[i]-'a'].Dugum;       /* yeni nesneyi kok dugum yap */
					}
				}
				else{
					if(i == kelime.Length - 1){                	 /* son karakter ise */
						if(Dugum[kelime[i]-'a'].Anlam == ""){  	 /* ilk eklenen anlam ise */     
							Dugum[kelime[i]-'a'].Anlam += anlam; /* son karakterin nesne referansina anlami ekle */
						}
						else{ 
							Dugum[kelime[i]-'a'].Anlam += ";" + anlam;/* son karakterin nesne referansina diger anlami ekle */
						}
					}
					Dugum = Dugum[kelime[i]-'a'].Dugum;        	 /* onceden eklenmis karakter var ise bir sonraki dugume git */
				}
			}
			Dugum = Kok;
		}
		
		public void KelimeSil(string kelime){
			KelimeAgaci[] Kok = new KelimeAgaci[26];
			Kok = Dugum;
			for(int i = 0; i < kelime.Length; i++){
				if(Dugum[kelime[i]-'a'] == null){
					Console.WriteLine("[boyle bir kelime yok]");
					break;
				}
				else if(i == kelime.Length - 1 && Dugum[kelime[i]-'a'].Anlam == ""){
					Console.WriteLine("[bu kelime daha once silinmis]");
					break;
				}
				else if(i == kelime.Length - 1)
					Dugum[kelime[i]-'a'].Anlam = "";
				Dugum = Dugum[kelime[i] - 'a'].Dugum;
			}
			Dugum = Kok;
		}
		
		public string AnlamBul(string kelime){
			KelimeAgaci[] Kok = new KelimeAgaci[26];
			Kok = Dugum;
			string temp = null;
			for(int i = 0; i < kelime.Length; i++){
				if(Dugum[kelime[i]-'a'] == null){
					Dugum = Kok;
					return "[kelime bulunamadi]";
				}
				else if(i == kelime.Length - 1 && Dugum[kelime[i]-'a'].Anlam == ""){
					Dugum = Kok;
					return "[bu kelime daha once silinmis]";
				}
				else if(i == kelime.Length - 1){
					temp = Dugum[kelime[i]-'a'].Anlam; /* anlami ara degiskende tut */
					Dugum = Kok;                       /* dugumu kok deger yap */
					break;
				}
				Dugum = Dugum[kelime[i] - 'a'].Dugum;  /* dugum uzerinde dolas, bir sonraki dugume git */
			}
			return temp;
		}
	}
	class MainClass{
		public static void Main (string[] args){
			KelimeAgaci sozluk = new KelimeAgaci();
			
			sozluk.KelimeEkle("legal", "yasal");
			sozluk.KelimeEkle("leg", "bacak");
			sozluk.KelimeEkle("a", "bir");
			sozluk.KelimeEkle("legend", "efsane");
			sozluk.KelimeEkle("leg", "dik kenar");
			
			Console.WriteLine("leg    : {0}", sozluk.AnlamBul("leg"));
			Console.WriteLine("bell   : {0}", sozluk.AnlamBul("bell"));
			Console.WriteLine("a      : {0}", sozluk.AnlamBul("a"));
			Console.WriteLine("legend : {0}", sozluk.AnlamBul("legend"));
			Console.WriteLine("legal  : {0}", sozluk.AnlamBul("legal"));
			
			sozluk.KelimeSil("legal");
			
			Console.WriteLine("legal : {0}", sozluk.AnlamBul("legal"));
			Console.ReadLine();		
		}
	}
}