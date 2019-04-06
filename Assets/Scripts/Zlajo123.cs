//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Zlajo123 : MonoBehaviour
//{

//    // Use this for initialization
//    void Start()
//    {
//        var o = new Oruzje("test")
//        {
//            Ime = "test",
//        };


//        var p = new Puška();

//        if (p.Pucaj()) {
//            HP=HP - 1;
//        }


//        p.Reload(); //desna


//        interface.text1.value = p.Metaka ;
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}



//class Oruzje
//{
//    public string Ime; //field
//    public string Ime2 { get; set; } //property (ima getter i setter) i da ih možeš kontrolirat pojedinačno
//    public string Ime3 { get { return Ime; } }



//    public Oruzje(string ime)
//    {

//    }


//    public void Metoda()
//    {
//    }
//}

//class DalekometnoOruzje : Oruzje
//{

//}

//class MeleeOruzje : Oruzje
//{


//}


//public class Puška
//{
//    private int _Metaka = 0;

//    public int Metaka { get { return _Metaka; } }



//   public void Reload()
//    {
//        _Metaka = 20;
//    }

//  public   bool Pucaj()
//    {
//        if (_Metaka > 0)
//        {
//            _Metaka--;
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }
//}