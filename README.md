#ScorpionEngine

###Getting started
In the Initialize method of your game class is where you initialize the engine (which I will be referring to as SE).
It needs the graphics device manager, content manager, and the name of the spritefont it will use as a console font. Leave this null if you don't wan't to use the game console.<br>
In your Update and Draw methods you need to respectively update and draw the SE too. The SE creates its own spritebatch, so you don't need to pass that as a parameter.
```
protected override void Initialize()
{
  SE.Initialize(graphics, Content, "consoleFont");
  base.Initialize();
}

protected override void Update(GameTime gameTime)
{
  SE.Update(gameTime);
  base.Update(gameTime);
}

protected override void Draw(GameTime gameTime)
{
  SE.Draw();
  base.Draw(gameTime);
}
```

###GameObjects and GameStates
By calling Update and Draw, the SE will update and draw all GameObjects in the current GameState. Objects in a gamestate other than the current one are not updated or drawn.

###Assets
Assets are loaded in dynamically when they are first used. Simply use the following code.
```
//Load the item
Assets.Load<Texture2D>("name");
//Get the item
Texture2D tex = Assets.Get<Texture2D>("name");
```
There is no need to load anything before getting it, because everything that isn't loaded when getting it will get loaded and stored, so it only needs to be loaded once. However, for sounds and music it can be useful to load it beforehand as this may cause lag.


Namespaces:  
ScorpionEngine<br>
.GameObjects<br>
.GameObjects.MenuItems<br>
.ParticleSystem<br>
.Shapes


Classes by namespace:  
- ScorpionEngine:  
SE (The main class)  
Assets  
Benchmark  
Camera2D  
Extensions  
GameConsole  
Input  
SpriteSheet

- ScorpionEngine.GameObjects:  
GameObject  
GameState

- ScorpionEngine.GameObjects.MenuItems:  
Button  
Slider  
ToggleButton

- ScorpionEngine.ParticleSystem:  
Particle  
ParticleEmitter  
ParticleModifier

- ScorpionEngine.Shapes:  
Line
