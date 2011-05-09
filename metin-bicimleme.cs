using System;
using System.IO;
using System.Text.RegularExpressions;

namespace odev1{
	public class Stack{
		private int maxsize;
		private string[] StackArray;
		private int index = -1;
		private string temp = null;         /* gecici olarak eleman tut */
		
		public Stack(int max){
			maxsize = max;
			StackArray = new string[maxsize];
		}
		public void Insert(string item, int i){
			if(index < maxsize - 1){
				for(; i <= index; i++){
					temp = StackArray[i];
					StackArray[i] = item;
				}
				StackArray[i+1] = item;
				index = i + 1;
			}
			else 
				Console.WriteLine("yigitin boyutunu asti yeterli yigit yok :(");
		}
		public void Push(string item){
			if(index < maxsize - 1)
				StackArray[++index] = item;
			else
				Console.WriteLine("yigitin boyutunu asti yeterli yigit yok :(");
		}
	
		public string Pop(){
			if(IsEmpty())
				return null;
			else {
				temp = StackArray[index];
				Delete();                      /* son elemani sil */
				return temp;
			}
		}
	
		public string Peek(){
			if(IsEmpty())
				return null;
			else 
				return StackArray[index];
		}
		
		public void Delete(){
			if( !IsEmpty())    /* stack eger bos degil ve gelen indis degeri de bos degilse sil */
				StackArray[index--] = null;
		}
		
		public bool IsEmpty(){
			if(index < 0) return true;
			else return false;
		}
		public int Length(){
			return index + 1;       /* boyut indisin 1 fazlasi */
	 	}
	}

	class OkumaYazma{
        public string Oku(string DosyaAdresi){
            StreamReader sr = new StreamReader(DosyaAdresi);
			string okunan = null;
            okunan = sr.ReadToEnd();
           	sr.Close();
            return okunan;
        }

		private string Buyuk(string str){
			return str.ToUpper();
		}
		
		private string Tekrarli(string str){
			string temp = "";
			for(int i = str.Length-1; i >= 0; i--){
				temp = temp.Insert(0, str[i].ToString());                 /* diziden karakterleri al ve 2 defa temp'e koy */
				temp = temp.Insert(0, str[i].ToString());
			}
			return temp;
		}
		
		private string Gizle(){
			return "";                       /* bos don */
		}
		
		private string Koseli(string str){
			return String.Concat("[", str, "]");
		}
		
		private string Duzenle(string str, char tag){
			string temp = "";
			int i;
			for (i = (str.Length-1); i >= 0; i--){
				if( str[i] != '>'){                                 /* en son acilan etikete kadar son dan basa oku */
					temp = temp.Insert(0, str[i].ToString());              /* acma tag belirtecine kadar olanlari temp e ekle */
				}
				else{	
					if(tag == 'u'){
						temp = Buyuk(temp);
						break;
					}
					if(tag == 'b'){
						temp = Tekrarli(temp);
						break;
					}
					if(tag == 'h'){
						temp = Gizle();
						break;
					}
					if(tag == 'p'){
						temp = Koseli(temp);
						break;
					}
				}
			}
			str = str.Remove(i);               /* bir tag in islemi bitince islenen veriyi gelen stringin ilgili bolumunu degistirmek icin sil */
			return String.Concat(str, temp);   /* degistirilen metni ekle ve don */
		}
		private bool state(int[] state_tag){
			for (int i = 0; i < state_tag.Length; i++){
				if(state_tag[i] != 0)
					return false;
			}
			return true;
		}
        public void Yaz(string metin){
			string[] tag = {"u","b","h","p"};
			int[] state_tag = {0,0,0,0};              /* acma taglarinin kapanmamis sayisini tut */
			string temp = "", gecici = "";
			string sonuc = "";
			int i;
		    for (i = 0; i < metin.Length; i++){                            /* metnin bastan sona oku */
		        if(metin[i] == '<' && metin[i+2] == '>'){
					temp = temp.Insert(temp.Length, metin[i+2].ToString());               /* acma tag i icin bir belirtec ekle */
					state_tag[Array.IndexOf(tag, metin[i+1].ToString())] += 1;            /* acma taglarinin kapanmamis sayisini artir */
					i += 2;
				}
				else if(metin[i] == '<' && metin[i+1] == '/' && metin[i+3] == '>'){ /* kapama tag i gelmisse kadar */
					gecici = Duzenle(temp, metin[i+2]);
					state_tag[Array.IndexOf(tag, metin[i+2].ToString())] -= 1;       /* acma taglarinin kapanmamis sayisini azalt */
					if(state(state_tag)){
						sonuc = sonuc.Insert(sonuc.Length, gecici);
						temp = "";
					}
					else 
						temp = gecici;
					i += 3;
				}
				else {                                                         /* karakter ise */
					temp = temp.Insert(temp.Length, metin[i].ToString());
				}
			}
			sonuc = sonuc.Insert(sonuc.Length, temp);
			Console.WriteLine("{0}",sonuc);
        }
    }
	
	public class Control{
		public static bool ParseChecker(string metin){
			Stack TagStack = new Stack(100);						   /* tag kontrolu icin yeni bir stack nesnesi olustur */
			
			MatchCollection tag;          						       /* duzenli ifadeye uygun stringlerin tutulacagi bir match dizisi */
			string[] opentag = {"<u>","<b>","<h>","<p>"};
			string[] closetag = {"</u>","</b>","</h>","</p>"};
			
			Regex RegexTag = new Regex("</?[a-zA-Z]>");  				/* duzenli ifade tanimla */
			tag = RegexTag.Matches(metin);        				        /* duzenli ifadye uygun olanları metinden bul */
			
			foreach (Match t in tag){
				if (t.Value[1] != '/')							        /* acma etiketlerini al */
					TagStack.Push(t.Value); 
				else{
					int i = Array.IndexOf(closetag, t.Value);           /* kapama etiketinin closetag listesindeki indexi */
					if(i < 0 || i != Array.IndexOf(opentag, TagStack.Pop())) /*etiket kapama ise stackte acma etiketi yoksa ya da etiket tanimli degilse */
						return false;
				}
			}
			return TagStack.IsEmpty();
		}
	}
	class MainClass{
		public static void Main (string[] args){
			string metin;
			
			OkumaYazma oy = new OkumaYazma();
			metin = oy.Oku("/home/seddi/Desktp/Dersler/NYP-C#/kodlar/kaynak.txt");
			
			if (Control.ParseChecker(metin)){
				oy.Yaz(metin);
			}
			else
				Console.WriteLine("Kaynak dosyanin bicimleme etiketleri hatalidir, kontrol ediniz.");
		}
	}
}

