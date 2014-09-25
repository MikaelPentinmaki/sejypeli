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
      
}

    void LuoKentta()
    {
        
        PhysicsObject VasenReuna = Level.CreateLeftBorder();
        PhysicsObject OikeaReuna = Level.CreateRightBorder();
        PhysicsObject YlaReuna = Level.CreateTopBorder();
        PhysicsObject AlaReuna = Level.CreateBottomBorder();

        VasenReuna.Height = 3;
        VasenReuna.Width = Screen.Height;

        OikeaReuna.Height = 3;
        OikeaReuna.Width = Screen.Height;

        YlaReuna.Height = 3;
        YlaReuna.Width = Screen.Height;

        AlaReuna.Height = 3;
        AlaReuna.Width = Screen.Height;

    }

    void AsetaNappaimet()
    {
        Keyboard.Listen(Key.Up, ButtonState.Down, LiikutaAlusta, "Liikuta Alusta");
        Keyboard.Listen(Key.Up, ButtonState.Released, PalautaKuva, "palauttaa aluksen kuvan");
        Keyboard.Listen(Key.Left, ButtonState.Down, KaannaAlustaVasempaan, "Käännä alusta vasempaan");
        Keyboard.Listen(Key.Right, ButtonState.Down, KaannaAlustaOikeaan, "Käännä alusta oikean");
    }

    void PalautaKuva()
    {
        Alus.Image = LoadImage("Alus");
    }


    void LiikutaAlusta()
    {
        Alus.Image = LoadImage("Alus2");
        Vector AlusSuunta = Vector.FromLengthAndAngle(50.0, Alus.Angle);
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

}
