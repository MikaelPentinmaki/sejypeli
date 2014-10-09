using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class Space_Gun_Man : PhysicsGame
{

    PhysicsObject Alus;
    Vector AlusSuunta;
    PhysicsObject Ammus;
    PhysicsObject Ammus2;
    PhysicsObject AlaReuna;
    PhysicsObject YlaReuna;
    PhysicsObject OikeaReuna;
    PhysicsObject VasenReuna;
    PhysicsObject Vihollinen;
    PhysicsObject AmmusV;
    IntMeter ELaskuri;
    Label KuolemaTeksti;
    IntMeter VELaskuri;
    IntMeter PLaskuri;
    Label PNaytto;
    
    public override void Begin()
    {
       LuoKentta();
       LuoAlus();
       AsetaNappaimet();
       LuoTausta();
       LuoVihollinen();
       VALaskuri();
       ElamaLaskuri();
       PisteLaskuri();

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    void LuoAlus()
{
    Alus = new PhysicsObject(5, 5);
    Image AlusKuva = LoadImage("Alus");
    Alus.Image = AlusKuva;
    
    Add(Alus);
    
    Camera.Zoom(7.0);
    Camera.FollowedObject = Alus;
    AddCollisionHandler(Alus, Tormaus);
    
}

    void LuoKentta()
    {
        
        VasenReuna = Level.CreateLeftBorder();
        OikeaReuna = Level.CreateRightBorder();
        YlaReuna = Level.CreateTopBorder();
        AlaReuna = Level.CreateBottomBorder();

        

    }

    void AsetaNappaimet()
    {
        Keyboard.Listen(Key.Up, ButtonState.Released, HidastaAlusta, "Hidasta alusta");
        Keyboard.Listen(Key.Up, ButtonState.Down, LiikutaAlusta, "Liikuta Alusta");
        Keyboard.Listen(Key.Up, ButtonState.Released, PalautaKuva, "palauttaa aluksen kuvan");
        Keyboard.Listen(Key.Left, ButtonState.Down, KaannaAlustaVasempaan, "Käännä alusta vasempaan");
        Keyboard.Listen(Key.Right, ButtonState.Down, KaannaAlustaOikeaan, "Käännä alusta oikean");
        Keyboard.Listen(Key.Space, ButtonState.Pressed, Ammu, "Ammu");
        Keyboard.Listen(Key.Down, ButtonState.Pressed, PysaytaAlus, "Pysäytä alus");
        
    }

    void PalautaKuva()
    {
        Alus.Image = LoadImage("Alus");
    }

    
    
    void LiikutaAlusta()
    {
        AlusSuunta = Vector.FromLengthAndAngle(50.0, Alus.Angle);
        Alus.Image = LoadImage("Alus2");
        Alus.Push(AlusSuunta);
        Alus.MaxVelocity = 300;
    }

    void KaannaAlustaVasempaan()
    {
        Alus.Angle += Angle.FromDegrees(3);
    }

    void KaannaAlustaOikeaan()
    {
        Alus.Angle += Angle.FromDegrees(-3);
    }

    void LuoTausta()
    {
        Image taustaKuva = LoadImage("TaustaKuva");
        Level.Background.Image = taustaKuva;
        Level.Background.TileToLevel();

        GameObject tausta = new GameObject(Screen.Width, Screen.Height);
        tausta.Image = taustaKuva;
        Add(tausta, -3);
        Layers[-3].RelativeTransition = new Vector(0.5, 0.5);
        
    }

    

    void Ammu()
    {
        Ammus = new PhysicsObject(0.4, 0.4);
        Ammus2 = new PhysicsObject(0.4, 0.4);
        Image AmmusKuva = LoadImage("Ammus Kuva");
        Ammus.Image = AmmusKuva;
        Ammus2.Image = AmmusKuva;
        Vector Suunta = Vector.FromLengthAndAngle(150.0, Alus.Angle);
        Angle Kulma = Alus.Angle;
        Kulma.Degrees -= 180;
        Vector Suunta2 = Vector.FromLengthAndAngle(150.0, Kulma);
        Ammus.Position = Alus.Position;
        Ammus2.Position = Alus.Position;
        Ammus.IgnoresCollisionResponse = true;
        Ammus2.IgnoresCollisionResponse = true;
        Ammus.Hit(Suunta);
        Ammus2.Hit(Suunta2);
        Add(Ammus);
        Add(Ammus2);
        AddCollisionHandler(Ammus, Vihollinen, OsuViholliseen);
        AddCollisionHandler(Ammus2, Vihollinen, OsuViholliseen);
    }
    
    void Tormaus(PhysicsObject tormaaja, PhysicsObject kohde)
    {
        if (kohde == YlaReuna)
        {
            double XK1 = tormaaja.X - tormaaja.X * 2;
            tormaaja.Position = new Vector(XK1, AlaReuna.Y + 55);
            
        }

        if (kohde == AlaReuna)
        {
            double XK2 = tormaaja.X - tormaaja.X * 2;
            tormaaja.Position = new Vector(XK2, YlaReuna.Y + -55);
        }

        if (kohde == VasenReuna)
        {
            double YK3 = tormaaja.Y - tormaaja.Y * 2;
            tormaaja.Position = new Vector(YK3, OikeaReuna.X + -55);
        }

        if (kohde == OikeaReuna)
        {
            double YK4 = tormaaja.Y - tormaaja.Y * 2;
            tormaaja.Position = new Vector(YK4, VasenReuna.X + 55);
        }
    }

    void LuoVihollinen()
    {
        Vihollinen = new PhysicsObject(5, 5);
        Vihollinen.X = -490;
        Image VihollisenKuva = LoadImage("Vihollis Alus");
        Vihollinen.Image = VihollisenKuva;
        FollowerBrain SeuraajaTekoäly = new FollowerBrain(Alus);
        SeuraajaTekoäly.TurnWhileMoving = true;
        SeuraajaTekoäly.Speed = 50.0;
        Vihollinen.Brain = SeuraajaTekoäly;
        Add(Vihollinen);
        VELaskuri = new IntMeter(5);
    }

    void VALaskuri()
    {
        Timer Laskuri = new Timer();
        Laskuri.Start();
        Laskuri.Timeout += VihollisenAmpuminen;
        Laskuri.Timeout += Laskuri.Start;
    }

    void VihollisenAmpuminen()
    {
        AmmusV = new PhysicsObject(0.4, 0.4);
        Image AmmusKuva = LoadImage("Ammus Kuva");
        AmmusV.Image = AmmusKuva;
        Vector Suunta = Vector.FromLengthAndAngle(150.0, Vihollinen.Angle);
        Angle Kulma2 = Vihollinen.Angle;
        Kulma2.Degrees -= 180;
        Vector Suunta2 = Vector.FromLengthAndAngle(150.0, Kulma2);
        AmmusV.Position = Vihollinen.Position;
        AmmusV.IgnoresCollisionResponse = true;
        AmmusV.Hit(Suunta);
        Add(AmmusV);
        AddCollisionHandler(Alus, AmmusV, VTormays);
    }

    void ElamaLaskuri()
    {
        ELaskuri = new IntMeter(20);

        Label PisteNaytto = new Label();
        PisteNaytto.X = Screen.Left + 1800;
        PisteNaytto.Y = Screen.Top - 100;
        PisteNaytto.TextColor = Color.Red;

        PisteNaytto.BindTo(ELaskuri);
        PisteNaytto.Title = "Life";
        Add(PisteNaytto);

        
    }

    void VTormays(PhysicsObject tormaaja, PhysicsObject kohde)
    {
        AmmusV.Destroy();
        ELaskuri.Value -= 1;
        if (ELaskuri.Value == 0)
        {
            TuhoaAlus();
        }
    }


    void OsuViholliseen(PhysicsObject tormaaja, PhysicsObject kohde)
    {
        
        Ammus.Destroy();
        Ammus2.Destroy();
        VELaskuri.Value -= 1;
        if (VELaskuri == 0)
        {
            TuhoaVihollinen();
        }
    }

    void HidastaAlusta()
    {
        Alus.MaxVelocity = 50;
    }

    void TuhoaAlus()
    {
        Alus.Destroy();
        KuolemaTeksti = new Label("You Died");
        KuolemaTeksti.TextColor = Color.Red;
        Add(KuolemaTeksti);
    }

    void TuhoaVihollinen()
    {
        Vihollinen.Destroy();
        LuoVihollinen();
        PLaskuri.Value += 1;
    }

    void PysaytaAlus()
    {
        Alus.MaxVelocity = 10;
    }

    void PisteLaskuri()
    {
        PLaskuri = new IntMeter(0);

        PNaytto = new Label();
        PNaytto.X = Screen.Left + 200;
        PNaytto.Y = Screen.Top - 100;
        PNaytto.TextColor = Color.Red;
        PNaytto.BindTo(PLaskuri);
        PNaytto.Title = "Destroyed enemies";
        Add(PNaytto);
    }
}
