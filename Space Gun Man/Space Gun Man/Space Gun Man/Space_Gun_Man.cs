﻿using System;
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
    Timer IsoAmmusLaskuri;
    PhysicsObject IsoAmmus;
    Image AmmusKuva = LoadImage("Ammus Kuva");
    Vector Suunta;
    bool SaakoAmpua = false;
    PhysicsObject Asteroidi1;
    PhysicsObject Asteroidi2;
    PhysicsObject Asteroidi3;
    Image AsteroidiKuva;
    Image AlusKuva;
    
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
        LuoIsoAmpumisLaskuri();
        LuoAsteroidit();

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, Exit, "Lopeta peli");
    }

    void LuoAlus()
    {
        Alus = new PhysicsObject(5, 5);
        AlusKuva = LoadImage("Alus");
        Alus.Image = AlusKuva;
    
        Add(Alus);
    
        Camera.Zoom(7.0);
        Camera.FollowedObject = Alus;
        AddCollisionHandler(Alus, Tormaus);
        Alus.CanRotate = false;
        
    }

    void LuoKentta()
    {
        
        VasenReuna = Level.CreateLeftBorder();
        OikeaReuna = Level.CreateRightBorder();
        YlaReuna = Level.CreateTopBorder();
        AlaReuna = Level.CreateBottomBorder();

        /*VasenReuna.IsVisible = false;
        OikeaReuna.IsVisible = false;
        YlaReuna.IsVisible = false;
        AlaReuna.IsVisible = false;*/
        VasenReuna.Color = Color.FromHexCode("050505");
        OikeaReuna.Color = Color.FromHexCode("050505");
        YlaReuna.Color = Color.FromHexCode("050505");
        AlaReuna.Color = Color.FromHexCode("050505");
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
        Keyboard.Listen(Key.LeftAlt, ButtonState.Pressed, AmmuIsoAmmus, "Ammu iso ammus");
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
            Ammus.Image = AmmusKuva;
            Ammus2.Image = AmmusKuva;
            Suunta = Vector.FromLengthAndAngle(150.0, Alus.Angle);
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
            AddCollisionHandler(Ammus, VasenReuna, TuhoaAmmus);
            AddCollisionHandler(Ammus, OikeaReuna, TuhoaAmmus);
            AddCollisionHandler(Ammus, YlaReuna, TuhoaAmmus);
            AddCollisionHandler(Ammus, AlaReuna, TuhoaAmmus);

            AddCollisionHandler(Ammus2, VasenReuna, TuhoaAmmus2);
            AddCollisionHandler(Ammus2, OikeaReuna, TuhoaAmmus2);
            AddCollisionHandler(Ammus2, YlaReuna, TuhoaAmmus2);
            AddCollisionHandler(Ammus2, AlaReuna, TuhoaAmmus2);
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

    void LuoIsoAmpumisLaskuri()
    {
        IsoAmmusLaskuri = new Timer();
        IsoAmmusLaskuri.Interval = 5;
        IsoAmmusLaskuri.Timeout += AmmuIsoAmmusNappaimet;
        IsoAmmusLaskuri.Start(0);
    }

    void AmmuIsoAmmusNappaimet()
    {
        SaakoAmpua = true;
        IsoAmmusLaskuri.Start(0);
        AmmusLaskuri.Value -= 10;
    }

    void AmmuIsoAmmus()
    {
        if (SaakoAmpua == true)
        {
            IsoAmmus = new PhysicsObject(4, 4);
            IsoAmmus.Image = AmmusKuva;
            Suunta = Vector.FromLengthAndAngle(150.0, Alus.Angle);
            IsoAmmus.Position = Alus.Position;
            IsoAmmus.IgnoresCollisionResponse = true;
            IsoAmmus.Hit(Suunta);
            Add(IsoAmmus);
            AddCollisionHandler(IsoAmmus, Vihollinen, OsuIsostiViholliseen);
            AddCollisionHandler(IsoAmmus, VasenReuna, TuhoaIsoAmmus);
            AddCollisionHandler(IsoAmmus, OikeaReuna, TuhoaIsoAmmus);
            AddCollisionHandler(IsoAmmus, YlaReuna, TuhoaIsoAmmus);
            AddCollisionHandler(IsoAmmus, AlaReuna, TuhoaIsoAmmus);
            SaakoAmpua = false;
            AddCollisionHandler(IsoAmmus, "Asteroidi", TuhoaAsteroidi);
        }
    }

    void OsuIsostiViholliseen(PhysicsObject tormaaja, PhysicsObject kohde)
    {
        IsoAmmus.Destroy();
        TuhoaVihollinen();
    }

    void TuhoaIsoAmmus(PhysicsObject tormaaja, PhysicsObject kohde)
    {
        tormaaja.Destroy();
    }

    void LuoAsteroidit()
    {
        AsteroidiKuva = LoadImage("Asteroidi");

        Asteroidi1 = new PhysicsObject(15, 15);
        Asteroidi1.X = -30;
        Asteroidi1.Y = -200;
        Asteroidi1.Tag = "Asteroidi";
        AddCollisionHandler(Alus, Asteroidi1, MenetaElamaa);
        Asteroidi1.Mass = 25;
        Asteroidi1.Image = AsteroidiKuva;
        //AddCollisionHandler(IsoAmmus, Asteroidi1, TuhoaAsteroidi);
        Add(Asteroidi1);

        Asteroidi2 = new PhysicsObject(15, 15);
        Asteroidi2.X = -100;
        Asteroidi2.Y = 300;
        AddCollisionHandler(Alus, Asteroidi2, MenetaElamaa);
        Asteroidi2.Mass = 25;
        Asteroidi2.Image = AsteroidiKuva;
        Asteroidi2.Tag = "Asteroidi";
        Add(Asteroidi2);

        Asteroidi3 = new PhysicsObject(15, 15);
        Asteroidi3.X = 300;
        Asteroidi3.Y = 10;
        AddCollisionHandler(Alus, Asteroidi3, MenetaElamaa);
        Asteroidi3.Mass = 25;
        Asteroidi3.Image = AsteroidiKuva;
        Asteroidi3.Tag = "Asteroidi";
        Add(Asteroidi3);
    }

    void MenetaElamaa(PhysicsObject tormaaja, PhysicsObject kohde)
    {
        ELaskuri.Value -= 5;
        if (ELaskuri.Value == 0)
        {
            TuhoaAlus();
        }
    }

    void TuhoaAsteroidi(PhysicsObject tormaaja, PhysicsObject kohde)
    {
        kohde.Destroy();
        tormaaja.Destroy();
    }

    void TuhoaAmmus(PhysicsObject tormaaja, PhysicsObject kohde)
    {
        tormaaja.Destroy();
    }

    void TuhoaAmmus2(PhysicsObject tomaaja, PhysicsObject kohde)
    {
        tomaaja.Destroy();
    }

}
