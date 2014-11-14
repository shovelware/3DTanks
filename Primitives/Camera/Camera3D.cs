using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Primitives
{
    class Camera3D
    {
        /*TODO:
         * Follow
         * Lookat
        */

        #region MVars

        Vector3 vPos, vDir, vUp, vTarget,
                IvPos, IvUp, IvTarget,
                vMove;

        float fov, aspect, 
              fovTar, aspectTar,
              Ifov, Iaspect,
              near, far,
              fSpeed, fSpeedMult,
              IfSpeed,
              fRotat, fSens;

        Quaternion qRot;

        Matrix mView, mRot, mProj;

        public Vector3 Position { get { return vPos; } set { vPos = value; } }

        public float FoV { get { return fov; } }
        public float FoVTarget { set { fovTar = value; } }

        public float Aspect { get { return aspect; } }
        public float AspectTarget { set { aspectTar = value; } }

        public Matrix Projection { get { return mProj; } set { mProj = value; } }
        public Matrix View { get { return mView; } set { mView = value; } }

        #endregion

        #region Make, Break

        public Camera3D(Vector3 position, Vector3 target, Vector3 up, float foV, float aspectratio, float nearplane, float farplane)
        {
            IvPos = vPos = position;
            IvTarget = vTarget = target;
            IvUp = vUp = up;

            vDir = Vector3.Normalize(target - position);

            Ifov = fovTar = fov = foV;

            Iaspect = aspectTar = aspect = aspectratio;
            near = nearplane;
            far = farplane;

            IfSpeed = fSpeed = 0.5f;
            fSpeedMult = 4;
            qRot = Quaternion.Identity;
        }

        public void Reset()
        {
            vPos = IvPos;
            vTarget = IvTarget;
            vUp = IvUp;

            vDir = Vector3.Normalize(vTarget - vPos);

            fovTar = fov = Ifov;
            aspectTar = aspect = Iaspect;

            mView = Matrix.Identity;
            mProj = Matrix.CreatePerspectiveFieldOfView(fov, aspect, near, far);
            qRot = Quaternion.Identity;
        }

        #endregion

        #region Tweaks

        private void LerpFoV()
        {
            if (fov != fovTar)
                fov += (fovTar - fov) * 0.1f;
        }

        private void LerpAspect()
        {
            aspect += (aspectTar - aspect) * 0.1f;
        }

        public void AspectReset()
        {
            if (aspect != aspectTar)
                aspectTar = Iaspect;
        }

        public void FoVReset()
        {
            if (fov != fovTar)
            fovTar = Ifov;
        }

        #endregion

        #region Movement

        public void MoveFore()
        {
            vMove += vDir;
        }

        public void MoveBack()
        {
            vMove -= vDir;
        }

        public void MoveLeft()
        {
            vMove -= Vector3.Cross(vDir, vUp);
        }

        public void MoveRight()
        {
            vMove += Vector3.Cross(vDir, vUp);
        }

        public void MoveUp()
        {
            vMove += vUp;
        }

        public void MoveDown()
        {
            vMove -= vUp;
        }

        public void MoveFast()
        {
            fSpeed *= fSpeedMult;
        }

        public void MoveSlow()
        {
            fSpeed /= fSpeedMult;
        }

        public void MoveRefresh()
        {
            fSpeed = IfSpeed;
            vMove = Vector3.Zero;
        }

        #endregion

        #region Rotation

        #region Local Axes

        public void RotateX(float rotation)
        {
            Quaternion qX = Quaternion.CreateFromAxisAngle(Vector3.Cross(vDir, Vector3.Up), rotation);
            qRot = Quaternion.Normalize(Quaternion.Concatenate(qRot, qX));
            UpdateDir();
        }

        public void RotateY(float rotation)
        {
            Quaternion qY = Quaternion.CreateFromAxisAngle(vUp, rotation);
            qRot = Quaternion.Normalize(Quaternion.Concatenate(qRot, qY));
            UpdateDir();
        }

        public void RotateZ(float rotation)
        {
            Quaternion qZ = Quaternion.CreateFromAxisAngle(vDir, rotation);
            qRot = Quaternion.Normalize(Quaternion.Concatenate(qRot, qZ));
            UpdateDir();
        }

        #endregion

        #region World Axes

        public void RotateWorldX(float rotation)
        {
            Quaternion qWorldX = Quaternion.CreateFromAxisAngle(Vector3.Right, rotation);
            qRot = Quaternion.Normalize(Quaternion.Concatenate(qRot, qWorldX));
            UpdateDir();
        }

        public void RotateWorldY(float rotation)
        {
            Quaternion qWorldY = Quaternion.CreateFromAxisAngle(Vector3.Up, rotation);
            qRot = Quaternion.Normalize(Quaternion.Concatenate(qRot, qWorldY));
            UpdateDir();
        }

        public void RotateWorldZ(float rotation)
        {
            Quaternion qWorldZ = Quaternion.CreateFromAxisAngle(Vector3.Forward, rotation);
            qRot = Quaternion.Normalize(Quaternion.Concatenate(qRot, qWorldZ));
            UpdateDir();
        }

        #endregion

        #endregion

        #region Intelligent

        public void Follow(Vector3 target)
        {
            vPos += (target - vPos) * 0.1f;//
        }

        public void Lookat(Vector3 target)
        {
            vDir = target;
        }

        #endregion

        #region Update

        private void UpdateDir()
        {
            qRot.Z = 0;
            mRot = Matrix.CreateFromQuaternion(qRot);
            vDir = Vector3.Normalize(Vector3.Transform(vDir, mRot));
            vUp = Vector3.Normalize(Vector3.Transform(vUp, mRot));
        }

        public void UpdateView()
        {
            mView = Matrix.CreateLookAt(vPos, vPos + vDir, vUp);
        }

        public void UpdateProjection()
        {
            mProj = Matrix.CreatePerspectiveFieldOfView(fov, aspect, near, far);
        }

        public void Update(GameTime gameTime)
        {
            vPos += vMove * fSpeed * gameTime.ElapsedGameTime.Milliseconds;
            MoveRefresh();

            LerpFoV();
            LerpAspect();

            UpdateView();
            UpdateProjection();
            qRot = Quaternion.Identity;
        }

        #endregion
    }
}
