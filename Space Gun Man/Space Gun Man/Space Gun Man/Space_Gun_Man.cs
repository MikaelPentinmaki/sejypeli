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
    
    public override void Begin()
    {
        LuoKentta();
       LuoAlus();
       AsetaNappaimet();
       LuoTausta();

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
        Keyboard.Listen(Key.Up, ButtonState.Down, LiikutaAlusta, "Liikuta Alusta");
        Keyboard.Listen(Key.Up, ButtonState.Released, PalautaKuva, "palauttaa aluksen kuvan");
        Keyboard.Listen(Key.Left, ButtonState.Down, KaannaAlustaVasempaan, "Käännä alusta vasempaan");
        Keyboard.Listen(Key.Right, ButtonState.Down, KaannaAlustaOikeaan, "Käännä alusta oikean");
        Keyboard.Listen(Key.Space, ButtonState.Pressed, Ammu, "Ammu");
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
}
