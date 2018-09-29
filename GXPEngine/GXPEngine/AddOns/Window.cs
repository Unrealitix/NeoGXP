using GXPEngine.OpenGL;
using GXPEngine.Core;

namespace GXPEngine {
	/// <summary>
	/// A class that can be used to create "sub windows" (e.g. mini-map, splitscreen, etc).
	/// This is not a gameobject. Instead, subscribe the RenderWindow method to the main game's 
	/// OnAfterRender event.
	/// </summary>
	class Window {
		/// <summary>
		/// The x coordinate of the window's left side
		/// </summary>
		public int windowX {
			get {
				return _windowX;
			}
			set {
				_windowX = value;
				_dirty = true;
			}
		}
		/// <summary>
		/// The y coordinate of the window's top
		/// </summary>
		public int windowY {
			get {
				return _windowY;
			}
			set {
				_windowY = value;
				_dirty = true;
			}
		}
		/// <summary>
		/// The window's width
		/// </summary>
		public int width {
			get {
				return _width;
			}
			set {
				_width = value;
				_dirty = true;
			}
		}
		/// <summary>
		/// The window's height
		/// </summary>
		public int height {
			get {
				return _height;
			}
			set {
				_height = value;
				_dirty = true;
			}
		}

		/// <summary>
		/// The game object (which should be in the hierarchy!) that determines the focus point, rotation and scale
		/// of the viewport window.
		/// </summary>
		public GameObject camera;
		/// <summary>
		/// everything in the hierarchy below this object is rendered to the window.
		/// typically: use the main game.
		/// </summary>
		public GameObject sceneRoot = Game.main;


		// private variables:
		int _windowX, _windowY;
		int _width, _height;
		bool _dirty=true;

		Transformable window;

		/// <summary>
		/// Creates a render window in the rectangle given by x,y,width,height.
		/// The camera determines the focal point, rotation and scale of this window.
		/// </summary>
		public Window(int x, int y, int width, int height, GameObject camera, GameObject sceneRoot=null) {
			_windowX = x;
			_windowY = y;
			_width = width;
			_height = height;
			this.camera = camera;
			this.sceneRoot = sceneRoot;
			if (this.sceneRoot == null) {
				this.sceneRoot = Game.main;
			}
			window = new Transformable ();
		}

		/// <summary>
		/// To render the scene in this window, subscribe this method to the main game's OnAfterRender event.
		/// </summary>
		public void RenderWindow(GLContext glContext) {
			Game.main.SetViewport (_windowX, _windowY, _width, _height);

			if (_dirty) {
				window.x = _windowX + _width / 2;
				window.y = _windowY + _height / 2;
				_dirty = false;
			}
			glContext.PushMatrix (window.matrix);

			int pushes = 1;
			GameObject current = camera;
			Transformable cameraInverse;
			while (current!=null && current!=sceneRoot) {
				cameraInverse = current.Inverse ();
				glContext.PushMatrix (cameraInverse.matrix);
				pushes++;
				current = current.parent;
			}
			if (current != null) {
				cameraInverse = current.Inverse ();
				glContext.PushMatrix (cameraInverse.matrix);
				pushes++;
			}

			GL.Clear(GL.COLOR_BUFFER_BIT);
			sceneRoot.Render (glContext);
			for (int i=0; i<pushes; i++) {
				glContext.PopMatrix ();
			}
			Game.main.SetViewport (0, 0, Game.main.width, Game.main.height);
		}
	}
}
