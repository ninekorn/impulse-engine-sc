using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using SharpDX;
using SharpDX.DirectInput;
using System.Windows.Media.Media3D;
using System.Windows;
using System.Windows.Media;

using Matrix = SharpDX.Matrix;
using Quaternion = SharpDX.Quaternion;
using Segment = SCCoreSystems.SC_Segment.Segment;
using System.Threading;

using System;
using System.Reflection;

namespace SCCoreSystems
{
    class SC_Update : SC_Intermediate_Update
    {



        Stopwatch timeWatch = new Stopwatch();

        Matrix _viewMatrix;
        Matrix _projectionMatrix;
        DMatrixBuffer[] arrayOfMatrixBuff = new DMatrixBuffer[1];
        float speed = 0.05f;
        float theTimer = 0;

        Stopwatch timeStopWatch = new Stopwatch();
        int watchSwitch = 0;

        Task tsk;
        DateTime time1;
        DateTime time2;


        protected override void SC_Init_DirectX()
        {

            stopwatch.Start();

            time1 = DateTime.Now;
            time2 = DateTime.Now;


            timeStopWatch.Stop();
            timeStopWatch.Reset();
            timeStopWatch.Start();


            /*if (watchSwitch == 0)
            {
                timeStopWatch.Stop();
                timeStopWatch.Reset();
                timeStopWatch.Start();
                watchSwitch = 1;
            }
            deltaTime = timeStopWatch.Elapsed.Milliseconds;*/
            tsk = DoWork(1);

            base.SC_Init_DirectX();
        }

        /* async Task DoWork(int timeOut)
        {
            time1 = DateTime.Now;
            time2 = DateTime.Now;
        _threadLoop:

            time2 = DateTime.Now;
            deltaTime = (time2.Ticks - time1.Ticks)* 0.00000001f; //100000000f // / 1000000000f

            time1 = time2;

            await Task.Delay(timeOut);
            Thread.Sleep(1);
            goto _threadLoop;
        }*/
        /*public async Task DoWork(int timeOut)
        {
            time1 = DateTime.Now;
            time2 = DateTime.Now;
        _threadLoop:

            //float startTime = (float)(timeStopWatch.ElapsedMilliseconds / 1000d);
            //float DeltaTimer = (float)Math.Abs((timeStopWatch.ElapsedMilliseconds / 1000d) - startTime);
            ///deltaTime += DeltaTimer;

            time2 = DateTime.Now;
            deltaTime = (time2.Ticks - time1.Ticks) / 10000000f; //100000000f

            time1 = time2;

            await Task.Delay(timeOut);
            Thread.Sleep(1);
            goto _threadLoop;
        }*/



        float _delta_timer_frame = 0;
        float _delta_timer_time = 0;
        float deltaTime;
        Stopwatch timeStopWatch00 = new Stopwatch();
        Stopwatch timeStopWatch01 = new Stopwatch();
        int _swtch = 0;
        int _swtch_counter_00 = 0;
        int _swtch_counter_01 = 0;
        int _swtch_counter_02 = 0;
        
        public async Task DoWork(int timeOut)//public void DoWork(int timeOut) //async Task
        {
            float startTime = (float)(timeStopWatch00.ElapsedMilliseconds);
        _threadLoop:

            if (_swtch == 0 || _swtch == 1)
            {
                if (_swtch == 0)
                {
                    if (_swtch_counter_00 >= 0)
                    {
                        ////////////////////
                        //UPGRADED DELTATIME
                        ////////////////////
                        //IMPORTANT PHYSICS TIME 
                        timeStopWatch00.Start();
                        time1 = DateTime.Now;
                        ////////////////////
                        //UPGRADED DELTATIME
                        ////////////////////
                        _swtch = 1;
                        _swtch_counter_00 = 0;
                    }
                }
                else if (_swtch == 1)
                {
                    if (_swtch_counter_01 >= 0)
                    {
                        ////////////////////
                        //UPGRADED DELTATIME
                        ////////////////////
                        timeStopWatch01.Start();
                        time2 = DateTime.Now;
                        ////////////////////
                        //UPGRADED DELTATIME
                        ////////////////////
                        _swtch = 2;
                        _swtch_counter_01 = 0;
                    }
                }
                else if (_swtch == 2)
                {

                }
            }

            //FRAME DELTATIME
            //_delta_timer_frame = (float)Math.Abs((timeStopWatch01.Elapsed.Ticks - timeStopWatch00.Elapsed.Ticks)) * 100000000f;

            //time2 = DateTime.Now;
            //_delta_timer_time = (time2.Ticks - time1.Ticks) * 100000000f; //100000000f
            //time1 = time2;

            //deltaTime = (float)Math.Abs(_delta_timer_time - _delta_timer_frame);

            _delta_timer_frame = (float)Math.Abs((timeStopWatch01.Elapsed.Ticks - timeStopWatch00.Elapsed.Ticks)) / 100000000f; //10000000000f

            time2 = DateTime.Now;
            _delta_timer_time = (time2.Ticks - time1.Ticks) / 100000000; //100000000f //10000000000f
            time1 = time2;

            deltaTime = (float)Math.Abs(_delta_timer_time - _delta_timer_frame);

            //time1 = time2;
            //await Task.Delay(1);
            //Thread.Sleep(timeOut);
            _swtch_counter_00++;
            _swtch_counter_01++;
            _swtch_counter_02++;


            await Task.Delay(timeOut);
            Thread.Sleep(1);
            goto _threadLoop;
        }

        Stopwatch stopwatch = new Stopwatch();

        protected override void Update()
        {
            //theTimer = deltaTime;
            impulse.dt = 1 / 60f;// 1 / 60f;// deltaTime;// 1 / 60f;// deltaTime;

            Camera.Render();

            _viewMatrix = Camera.ViewMatrix;
            _projectionMatrix = proj;

            //Model.Render(device.ImmediateContext);
            //ColorShader.Render(context, Model.IndexCount, matroxer1, _viewMatrix, _projectionMatrix);

            UpdateRigidBodies(theTimer);

            ReadKeyboard();

            if (_KeyboardState != null && _KeyboardState.PressedKeys.Contains(Key.Up))
            {
                viewerPos.Z += speed;
            }
            else if (_KeyboardState != null && _KeyboardState.PressedKeys.Contains(Key.Down))
            {
                viewerPos.Z -= speed;
            }
            else if (_KeyboardState != null && _KeyboardState.PressedKeys.Contains(Key.Q))
            {
                viewerPos.Y += speed;
            }
            else if (_KeyboardState != null && _KeyboardState.PressedKeys.Contains(Key.Z))
            {
                viewerPos.Y -= speed;
            }
            else if (_KeyboardState != null && _KeyboardState.PressedKeys.Contains(Key.Left))
            {
                viewerPos.X -= speed;
            }
            else if (_KeyboardState != null && _KeyboardState.PressedKeys.Contains(Key.Right))
            {
                viewerPos.X += speed;
            }

            Camera.SetPosition(viewerPos.X, viewerPos.Y, viewerPos.Z);
            //deltaTime = timeStopWatch.Elapsed.Milliseconds;
        }

        void UpdateRigidBodies(float theTime)
        {
            //impulse.step();
            //impulse.step();

            accumulator += deltaTime;// timeStopWatch.Elapsed.Seconds;// Time.deltaTime;// state.seconds;

            if (accumulator >= impulse.dt)
            {
                impulse.step();
                accumulator -= impulse.dt;
            }

            for (int i = 0; i < impulse.bodies.Count;i++)
            {        
                Body body = impulse.bodies[i];

                //body.shape.gameObject.position = new Vector2(body.position.x, body.position.y);

                //float posX = (float)Math.Round(body.position.x * 10) * 0.1f;
                //float posY = (float)Math.Round(body.position.x * 10) * 0.1f;

                if (body.shape.getType() == Shape.Type.Circle)
                {
                    if (body.shape.body.isStatic == 0)
                    {
                        body.shape.gameObjectCirc.position = new Vector2(body.position.x, body.position.y);

                        float rx = (float)Math.Cos(body.orient) * 2;
                        float ry = (float)Math.Sin(body.orient) * 2;
                        Vector2 dirToPoint = new Vector2(body.shape.gameObjectCirc.position.X + rx, body.shape.gameObjectCirc.position.Y + ry) - new Vector2(body.shape.gameObjectCirc.position.X, body.shape.gameObjectCirc.position.Y);
                        dirToPoint.Normalize();

                        Vector2 orientPos = new Vector2(body.shape.gameObjectCirc.position.X + rx, body.shape.gameObjectCirc.position.Y + ry);

                        rx = (float)Math.Cos(body.originOrient) * 2;
                        ry = (float)Math.Sin(body.originOrient) * 2;

                        Vector2 originDir = new Vector2(body.shape.gameObjectCirc.position.X + rx, body.shape.gameObjectCirc.position.Y + ry) - new Vector2(body.shape.gameObjectCirc.position.X, body.shape.gameObjectCirc.position.Y);
                        originDir.Normalize();

                        rx = (float)Math.Cos(body.lastOrient) * 2;
                        ry = (float)Math.Sin(body.lastOrient) * 2;

                        Vector2 lastOrient = new Vector2(body.shape.gameObjectCirc.position.X + rx, body.shape.gameObjectCirc.position.Y + ry);

                        //Vector3 a_normalized = new Vector3(dirToPoint.X, dirToPoint.Y, 0);
                        //Vector3 b_normalized = new Vector3(originDir.X, originDir.Y, 0);
                        Vector2 a_normalized2 = new Vector2(dirToPoint.X, dirToPoint.Y);
                        Vector2 b_normalized2 = new Vector2(originDir.X, originDir.Y);

                        float angleNorm = -(float)sc_maths.AngleBetween(a_normalized2, b_normalized2);

                        var angleDiff = angleNorm - body.shape.gameObjectCirc.OriginRotationZ;

                        float pitch = (body.shape.gameObjectCirc.OriginRotationX) * 0.0174532925f;
                        float yaw = (body.shape.gameObjectCirc.OriginRotationY) * 0.0174532925f;
                        float roll = (body.shape.gameObjectCirc.OriginRotationZ) * 0.0174532925f;
                        Matrix currentMatrix = SharpDX.Matrix.RotationYawPitchRoll(yaw, pitch, roll);

                        float pitcher = (body.shape.gameObjectCirc.CurrentRotationX) * 0.0174532925f;
                        float yawer = (body.shape.gameObjectCirc.CurrentRotationY) * 0.0174532925f;
                        float roller = (angleDiff) * 0.0174532925f;
                        Matrix currentMatrixer = SharpDX.Matrix.RotationYawPitchRoll(yawer, pitcher, roller);

                        Matrix matroxer2 = Matrix.Multiply(currentMatrix, currentMatrixer);

                        matroxer2.M41 = body.shape.gameObjectCirc.position.X;
                        matroxer2.M42 = body.shape.gameObjectCirc.position.Y;

                        body.shape.gameObjectCirc.originRot = matroxer2;

                        Matrix worldMatrix = Matrix.Identity;
                        worldMatrix = body.shape.gameObjectCirc.originRot;
                        //worldMatrix.M41 = body.shape.gameObject.position.X;
                        //worldMatrix.M42 = body.shape.gameObject.position.Y;

                        body.shape.gameObjectCirc.Render(device.ImmediateContext);
                        ColorShader.Render(context, body.shape.gameObjectCirc.IndexCount, worldMatrix, _viewMatrix, _projectionMatrix);
                    }
                }
                else if (body.shape.getType() == Shape.Type.Poly)
                {
                    if (body.shape.gameObject != null)
                    {
                        body.shape.gameObject.position = new Vector2(body.position.x, body.position.y);

                        float rx = (float)Math.Cos(body.orient) * 2;
                        float ry = (float)Math.Sin(body.orient) * 2;
                        Vector2 dirToPoint = new Vector2(body.shape.gameObject.position.X + rx, body.shape.gameObject.position.Y + ry) - new Vector2(body.shape.gameObject.position.X, body.shape.gameObject.position.Y);
                        dirToPoint.Normalize();

                        Vector2 orientPos = new Vector2(body.shape.gameObject.position.X + rx, body.shape.gameObject.position.Y + ry);

                        rx = (float)Math.Cos(body.originOrient) * 2;
                        ry = (float)Math.Sin(body.originOrient) * 2;

                        Vector2 originDir = new Vector2(body.shape.gameObject.position.X + rx, body.shape.gameObject.position.Y + ry) - new Vector2(body.shape.gameObject.position.X, body.shape.gameObject.position.Y);
                        originDir.Normalize();

                        rx = (float)Math.Cos(body.lastOrient) * 2;
                        ry = (float)Math.Sin(body.lastOrient) * 2;

                        Vector2 lastOrient = new Vector2(body.shape.gameObject.position.X + rx, body.shape.gameObject.position.Y + ry);

                        //Vector3 a_normalized = new Vector3(dirToPoint.X, dirToPoint.Y, 0);
                        //Vector3 b_normalized = new Vector3(originDir.X, originDir.Y, 0);
                        Vector2 a_normalized2 = new Vector2(dirToPoint.X, dirToPoint.Y);
                        Vector2 b_normalized2 = new Vector2(originDir.X, originDir.Y);

                        float angleNorm = -(float)sc_maths.AngleBetween(a_normalized2, b_normalized2);

                        var angleDiff = angleNorm - body.shape.gameObject.OriginRotationZ;

                        float pitch = (body.shape.gameObject.OriginRotationX) * 0.0174532925f;
                        float yaw = (body.shape.gameObject.OriginRotationY) * 0.0174532925f;
                        float roll = (body.shape.gameObject.OriginRotationZ) * 0.0174532925f;
                        Matrix currentMatrix = SharpDX.Matrix.RotationYawPitchRoll(yaw, pitch, roll);

                        float pitcher = (body.shape.gameObject.CurrentRotationX) * 0.0174532925f;
                        float yawer = (body.shape.gameObject.CurrentRotationY) * 0.0174532925f;
                        float roller = (angleDiff) * 0.0174532925f;
                        Matrix currentMatrixer = SharpDX.Matrix.RotationYawPitchRoll(yawer, pitcher, roller);

                        Matrix matroxer2 = Matrix.Multiply(currentMatrix, currentMatrixer);


                        matroxer2.M41 = body.shape.gameObject.position.X;
                        matroxer2.M42 = body.shape.gameObject.position.Y;

                        body.shape.gameObject.originRot = matroxer2;

                        body.setLastOrient(body.orient);
                        body.lastAngle = angleNorm;

                        Matrix worldMatrix = Matrix.Identity;
                        worldMatrix = body.shape.gameObject.originRot;

                        body.shape.gameObject.Render(device.ImmediateContext);
                        ColorShader.Render(context, body.shape.gameObject.IndexCount, worldMatrix, _viewMatrix, _projectionMatrix);

                    }
                    else
                    {

                        body.shape.gameObject_triangle.position = new Vector2(body.position.x, body.position.y);

                        float rx = (float)Math.Cos(body.orient) * 2;
                        float ry = (float)Math.Sin(body.orient) * 2;
                        Vector2 dirToPoint = new Vector2(body.shape.gameObject_triangle.position.X + rx, body.shape.gameObject_triangle.position.Y + ry) - new Vector2(body.shape.gameObject_triangle.position.X, body.shape.gameObject_triangle.position.Y);
                        dirToPoint.Normalize();

                        Vector2 orientPos = new Vector2(body.shape.gameObject_triangle.position.X + rx, body.shape.gameObject_triangle.position.Y + ry);

                        rx = (float)Math.Cos(body.originOrient) * 2;
                        ry = (float)Math.Sin(body.originOrient) * 2;

                        Vector2 originDir = new Vector2(body.shape.gameObject_triangle.position.X + rx, body.shape.gameObject_triangle.position.Y + ry) - new Vector2(body.shape.gameObject_triangle.position.X, body.shape.gameObject_triangle.position.Y);
                        originDir.Normalize();

                        rx = (float)Math.Cos(body.lastOrient) * 2;
                        ry = (float)Math.Sin(body.lastOrient) * 2;

                        Vector2 lastOrient = new Vector2(body.shape.gameObject_triangle.position.X + rx, body.shape.gameObject_triangle.position.Y + ry);

                        //Vector3 a_normalized = new Vector3(dirToPoint.X, dirToPoint.Y, 0);
                        //Vector3 b_normalized = new Vector3(originDir.X, originDir.Y, 0);
                        Vector2 a_normalized2 = new Vector2(dirToPoint.X, dirToPoint.Y);
                        Vector2 b_normalized2 = new Vector2(originDir.X, originDir.Y);

                        float angleNorm = -(float)sc_maths.AngleBetween(a_normalized2, b_normalized2);

                        var angleDiff = angleNorm - body.shape.gameObject_triangle.OriginRotationZ;

                        float pitch = (body.shape.gameObject_triangle.OriginRotationX) * 0.0174532925f;
                        float yaw = (body.shape.gameObject_triangle.OriginRotationY) * 0.0174532925f;
                        float roll = (body.shape.gameObject_triangle.OriginRotationZ) * 0.0174532925f;
                        Matrix currentMatrix = SharpDX.Matrix.RotationYawPitchRoll(yaw, pitch, roll);

                        float pitcher = (body.shape.gameObject_triangle.CurrentRotationX) * 0.0174532925f;
                        float yawer = (body.shape.gameObject_triangle.CurrentRotationY) * 0.0174532925f;
                        float roller = (angleDiff) * 0.0174532925f;
                        Matrix currentMatrixer = SharpDX.Matrix.RotationYawPitchRoll(yawer, pitcher, roller);

                        Matrix matroxer2 = Matrix.Multiply(currentMatrix, currentMatrixer);


                        matroxer2.M41 = body.shape.gameObject_triangle.position.X;
                        matroxer2.M42 = body.shape.gameObject_triangle.position.Y;

                        body.shape.gameObject_triangle.originRot = matroxer2;

                        body.setLastOrient(body.orient);
                        body.lastAngle = angleNorm;

                        Matrix worldMatrix = Matrix.Identity;
                        worldMatrix = body.shape.gameObject_triangle.originRot;

                        body.shape.gameObject_triangle.Render(device.ImmediateContext);
                        ColorShader.Render(context, body.shape.gameObject_triangle.IndexCount, worldMatrix, _viewMatrix, _projectionMatrix);
                    }
                }
            }
        }

        KeyboardState _KeyboardState;

        private bool ReadKeyboard()
        {
            var resultCode = SharpDX.DirectInput.ResultCode.Ok;
            _KeyboardState = new KeyboardState();

            try
            {
                // Read the keyboard device.
                _Keyboard.GetCurrentState(ref _KeyboardState);
            }
            catch (SharpDX.SharpDXException ex)
            {
                resultCode = ex.Descriptor; // ex.ResultCode;
            }
            catch (Exception)
            {
                return false;
            }

            // If the mouse lost focus or was not acquired then try to get control back.
            if (resultCode == SharpDX.DirectInput.ResultCode.InputLost || resultCode == SharpDX.DirectInput.ResultCode.NotAcquired)
            {
                try
                {
                    _Keyboard.Acquire();
                }
                catch
                { }

                return true;
            }

            if (resultCode == SharpDX.DirectInput.ResultCode.Ok)
                return true;

            return false;
        }
    }
}

/*Func<bool> formatDelegate = () =>
{
    return true;
};
var t2 = new Task<bool>(formatDelegate);
t2.RunSynchronously();
t2.Dispose();*/