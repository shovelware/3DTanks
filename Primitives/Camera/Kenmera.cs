using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Primitives
{
    public class Camera
    {
        Vector3 pos;
        Vector3 dir;
        Vector3 up;

        float fieldOfView ,aspectRatio ,near ,far;

        Matrix view, projection;

        public Matrix Projection { get { return projection; } set { projection = value; } }
        public Matrix View { get { return view; } set { view = value; } }

        float speed = 0.050f;
        float angVel = 0.0025f; // set max angular velocity for rotating camera

        public void Init(Vector3 p, Vector3 lookat, Vector3 u, float FOV, float ar, float n, float f)
        {
            pos = p;
            dir = (lookat - p);
            dir.Normalize();
            up = u;

            fieldOfView = FOV;
            aspectRatio = ar;
            near = n;
            far = f;
            UpdateView();
            UpdateProj();
        }
        void UpdateView()
        {
            view= Matrix.CreateLookAt(pos, pos + dir, up);
        }
        void UpdateProj()
        {
            projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, near, far);

        }

        public void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();

            float distanceTravelled = speed * gameTime.ElapsedGameTime.Milliseconds;
            
            Vector3 right = Vector3.Cross(dir, up);

 
            if (kb.IsKeyDown(Keys.Left))
            {// pan
                pos -= right * distanceTravelled;
            }
            if (kb.IsKeyDown(Keys.Right))
            {// pan
                pos += right * distanceTravelled;
            }
            
            if (kb.IsKeyDown(Keys.Up))
            {// forward
                pos += dir * distanceTravelled;
            }
            if (kb.IsKeyDown(Keys.Down))
            {// Back
                pos -= dir * distanceTravelled;
            }

            UpdateView();
            UpdateProj();
        }




    }
}
