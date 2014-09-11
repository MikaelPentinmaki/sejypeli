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

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    void LuoAlus()
{
    Alus = new PhysicsObject(16, 16);
    Image AlusKuva = LoadImage("Alus");
    Alus.Image = AlusKuva;
    Add(Alus);

}

    void LuoKentta()
    {
        Level.Background.CreateStars(1000);
    }

    void AsetaNappaimet()
    {
        
    }
}
