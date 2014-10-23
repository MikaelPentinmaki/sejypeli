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
    PhysicsObject ElamaPisteVoima1;
    PhysicsObject ElamaPisteVoima2;
    Image ElamaPisteVoimaKuva;
    IntMeter AmmusLaskuri;
    Label AmmusNaytto;
    PhysicsObject AmmusPisteVoima1;
    PhysicsObject AmmusPisteVoima2;
    Image AmmusPisteVoimaKuva;
    Label UudelleenSyntymaTeksti;
    
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
       LuoElamaPisteVoimat();
       LuoAmmusLaskuri();
       LuoAmmusVoimat();

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

    void TuhoaAlus()
    {
        Alus.Destroy();
        KuolemaTeksti = new Label("You Died");
        KuolemaTeksti.TextColor = Color.Red;
        Add(KuolemaTeksti);

        UudelleenSyntymaTeksti = new Label("Respawn");
        UudelleenSyntymaTeksti.Y = -30;
        UudelleenSyntymaTeksti.TextColor = Color.Red;
        Add(UudelleenSyntymaTeksti);
        Mouse.ListenOn(UudelleenSyntymaTeksti, MouseButton.Left, ButtonState.Pressed, AloitaAlusta, "Aloita Alusta");
    }

    void AsetaNappaimet()
    {
        Keyboard.Listen(Key.Up, ButtonState.Down, LiikutaAlusta, "Liikuta Alusta");
        Keyboard.Listen(Key.Up, ButtonState.Released, PalautaKuva, "palauttaa aluksen kuvan");
        Keyboard.Listen(Key.Left, ButtonState.Down, KaannaAlustaVasempaan, "Käännä alusta vasempaan");
        Keyboard.Listen(Key.Right, ButtonState.Down, KaannaAlustaOikeaan, "Käännä alusta oikean");
        Keyboard.Listen(Key.Space, ButtonState.Pressed, Ammu, "Ammu");
        Keyboard.Listen(Key.Down, ButtonState.Pressed, PysaytaAlus, "Pysäytä alus");
        IsMouseVisible = true;
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
        if (AmmusLaskuri.Value >= 1)
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
            AmmusLaskuri.Value -= 2;
        }
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
        Vihollinen.IgnoresCollisionResponse = true;
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

    void TuhoaVihollinen()
    {
        Vihollinen.Destroy();
        LuoVihollinen();
        PLaskuri.Value += 1;
    }

    void PysaytaAlus()
    {
        Alus.MaxVelocity = 20;
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

    void LuoElamaPisteVoimat()
    {
        ElamaPisteVoima1 = new PhysicsObject(4, 4);
        ElamaPisteVoimaKuva = LoadImage("+ElämäPisteVoima");
        ElamaPisteVoima1.Image = ElamaPisteVoimaKuva;
        ElamaPisteVoima1.X = 200;
        ElamaPisteVoima1.Y = 300;
        AddCollisionHandler(Alus, ElamaPisteVoima1, KeraaElamaPisteVoima1);
        ElamaPisteVoima1.IgnoresCollisionResponse = true;
        Add(ElamaPisteVoima1);

        ElamaPisteVoima2 = new PhysicsObject(4, 4);
        ElamaPisteVoima2.Image = ElamaPisteVoimaKuva;
        ElamaPisteVoima2.X = -200;
        ElamaPisteVoima2.Y = -200;
        AddCollisionHandler(Alus, ElamaPisteVoima2, KeraaElamaPisteVoima2);
        ElamaPisteVoima2.IgnoresCollisionResponse = true;
        Add(ElamaPisteVoima2);
    }

    void KeraaElamaPisteVoima1(PhysicsObject tormaaja, PhysicsObject kohde)
    {
        ElamaPisteVoima1.Destroy();
        ELaskuri.Value += 5;
    }

    void KeraaElamaPisteVoima2(PhysicsObject tormaaja, PhysicsObject kohde)
    {
        ElamaPisteVoima2.Destroy();
        ELaskuri.Value += 5;
    }

    void LuoAmmusLaskuri()
    {
        AmmusLaskuri = new IntMeter(500);

        AmmusNaytto = new Label();
        AmmusNaytto.X = Screen.Left + 1800;
        AmmusNaytto.Y = Screen.Top - 200;
        AmmusNaytto.TextColor = Color.Red;

        AmmusNaytto.BindTo(AmmusLaskuri);
        AmmusNaytto.Title = "Bullets";
        Add(AmmusNaytto);
    }

    void LuoAmmusVoimat()
    {
        AmmusPisteVoima1 = new PhysicsObject(4, 4);
        AmmusPisteVoimaKuva = LoadImage("+AmmusPisteVoima");
        AmmusPisteVoima1.Image = AmmusPisteVoimaKuva;
        AmmusPisteVoima1.X = 450;
        AmmusPisteVoima1.Y = -300;
        AddCollisionHandler(Alus, AmmusPisteVoima1, KeraaAmmusPisteVoima1);
        AmmusPisteVoima1.IgnoresCollisionResponse = true;
        Add(AmmusPisteVoima1);

        AmmusPisteVoima2 = new PhysicsObject(4, 4);
        AmmusPisteVoima2.Image = AmmusPisteVoimaKuva;
        AmmusPisteVoima2.X = -100;
        AmmusPisteVoima2.Y = 350;
        AddCollisionHandler(Alus, AmmusPisteVoima2, KeraaAmmusPisteVoima2);
        AmmusPisteVoima2.IgnoresCollisionResponse = true;
        Add(AmmusPisteVoima2);
    }

    void KeraaAmmusPisteVoima1(PhysicsObject tormaaja, PhysicsObject kohde)
    {
        AmmusPisteVoima1.Destroy();
        AmmusLaskuri.Value += 250;
    }

    void KeraaAmmusPisteVoima2(PhysicsObject tormaaja, PhysicsObject kohde)
    {
        AmmusPisteVoima2.Destroy();
        AmmusLaskuri.Value += 250;
    }

    void AloitaAlusta()
    {
        ClearAll();
        Begin();
    }
}
