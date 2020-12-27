using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;

using SharpDX;
using AC.Components.Util;
using System.Windows.Threading;

namespace SC_Console_APP
{
    //abstract
    public class WorkerThread //;: SC_ThreadPool
    {

        public static SC_DirectX.chunkData functionToCall; //Func<int>
        public static IAsyncResult resultPending;
        public static int startFunction = 1;
        public static bool hasPopped = false;
        public static int startPop = 1;
        public static int hasWorked = 0;
        public static int someSwitch = 1;
        public static ManualResetEvent _TME;

        public DShaderManager shaderManager;

        public ConcurrentStack<SC_DirectX.chunkData> queueOfFunctions = new ConcurrentStack<SC_DirectX.chunkData>();

        public Task tsk;
        SC_ThreadPool test = SC_ThreadPool.threadPool;
        public WorkerThread(int workerThreadID, ManualResetEvent TME)
        {
            queueOfFunctions = test.queueOfFunctions;
            this._workerThreadID = workerThreadID;
            tsk = DoWork(1, workerThreadID, TME);
            //Task.WaitAny(tsk);
            //Task<Result> task = Task.Run<Result>(async () => await DoWork(1, workerThreadID, TME));
            //Console.WriteLine("test");

            //tester(workerThreadID, TME);
            //hasPopped = queueOfFunctions.TryPop(out functionToCall);
            //var shaderChunk = functionToCall.chunkShader;
            //testanus<SC_VR_Chunk_Shader>();
        }



        public void testanus<T>() //int workerThreadID, ManualResetEvent TM
        {

            //return t;
        }

        /*public async void AsyncMethod() //int workerThreadID, ManualResetEvent TME
        {
            //MapThreadInfo<int> tester = new MapThreadInfo<int>(1);

            /*if (queueOfFunctions != null)
            {
                if (queueOfFunctions.Count > 0)
                {               
                    //Task<int> t;
                    ///t = shaderChunk.Render(functionToCall);
                    //await t;
                    //return t;
                    //MapThreadInfo<SC_VR_Chunk_Shader.RenderChunk> yo = new MapThreadInfo<SC_VR_Chunk_Shader.RenderChunk>();

                    SC_VR_Chunk_Shader.RenderChunk classOfChunk = new SC_VR_Chunk_Shader.RenderChunk(functionToCall);

                    //shaderChunk.Render(functionToCall)

                    //await Task.FromResult<T>(yo);
                    Task<int> t = new Task<int>();
                }
            }


            Task task = new Task(delegate { someOtherDummyMethod(); });
            task.Start();
            hasPopped = queueOfFunctions.TryPop(out functionToCall);
            var shaderChunk = functionToCall.chunkShader;
            //Task task = new Task(new Action(someOtherDummyMethod));
            //task.RunSynchronously();
            object test = new object();
            Complex a = 1;
            T x = MangeuxDeMarde(a);
            return x;
        }*/




        public T Get<T>(object type) where T : IConvertible
        {
            return (T)Convert.ChangeType(type, typeof(T));
        }

        public class Complex : IConvertible
        {
            TypeCode IConvertible.GetTypeCode()
            {
                throw new NotImplementedException();
            }

            bool IConvertible.ToBoolean(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            byte IConvertible.ToByte(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            char IConvertible.ToChar(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            DateTime IConvertible.ToDateTime(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            decimal IConvertible.ToDecimal(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            double IConvertible.ToDouble(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            short IConvertible.ToInt16(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            int IConvertible.ToInt32(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            long IConvertible.ToInt64(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            sbyte IConvertible.ToSByte(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            float IConvertible.ToSingle(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            string IConvertible.ToString(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            object IConvertible.ToType(Type conversionType, IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            ushort IConvertible.ToUInt16(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            uint IConvertible.ToUInt32(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            ulong IConvertible.ToUInt64(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }
        }








        /*private Task<SC_VR_Chunk_Shader> dummyMethod(int i)
        {
            throw new NotImplementedException();
        }


        public class R : IConvertible
        {
            TypeCode IConvertible.GetTypeCode()
            {
                throw new NotImplementedException();
            }

            bool IConvertible.ToBoolean(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            byte IConvertible.ToByte(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            char IConvertible.ToChar(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            DateTime IConvertible.ToDateTime(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            decimal IConvertible.ToDecimal(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            double IConvertible.ToDouble(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            short IConvertible.ToInt16(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            int IConvertible.ToInt32(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            long IConvertible.ToInt64(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            sbyte IConvertible.ToSByte(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            float IConvertible.ToSingle(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            string IConvertible.ToString(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            object IConvertible.ToType(Type conversionType, IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            ushort IConvertible.ToUInt16(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            uint IConvertible.ToUInt32(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            ulong IConvertible.ToUInt64(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }
        }*/












        /*public async Task dummyMethod<int>(Task t)
        {
            Action formatDelegate = () =>
            {
                someOtherDummyMethod();
            };

            Task task = new Task(formatDelegate);
            //T x = await formatDelegate();
            return task;
        }*/


        public void someOtherDummyMethod()
        {

        }


        public class MyClass<T>
        {
            public T iLeft { get; set; }
            public string sLeft { get { return this.iLeft.ToString(); } }

            public MyClass(T left)
            {
                iLeft = left;
            }

            public static implicit operator MyClass<T>(T left)
            {
                return new MyClass<T>(left);
            }

            public static implicit operator Int32(MyClass<T> left)
            {
                if (typeof(T).Equals(typeof(Int32)))
                {
                    return (int)(object)left.iLeft;
                }
                else
                {
                    return -1;
                }
            }
        }









        /*public T Get<T>(coliss type) where T : IConvertible
        {
            return (T)Convert.ChangeType(type, typeof(T));
        }




        struct MapThreadInfo<T>
        {
            //public Action<T> callback;
            //public T parameter;

            public MapThreadInfo(T parameter) //Action<T> callback, T parameter
            {
                //this.callback = callback;
                //this.parameter = parameter;
            }
        }



        public class coliss//<T>
        {
            public int _workerThreadID;
            public ManualResetEvent _TME;
            public int timeOut;
            public coliss(int _timeOut, int workerThreadID, ManualResetEvent TME)
            {
                this._workerThreadID = workerThreadID;
                this._TME = TME;
                this.timeOut = _timeOut;
            }

            /*public Task<T> tester()
            {
            Task<T> t = test();
            return t;
            }

        }*/





        //public ConcurrentQueue<> test = new ConcurrentQueue<>();

        /*public Result Execute(string parameters)
        {
            Task<Result> task = Task.Run<Result>(async () => await ExecuteAsync());
            return task.Result;
        }

        public async Task<Result> ExecuteAsync(string parameters)
        {
            Task<Result> result = SomeOtherOperationAsync();
            // more code here...
            return await result;
        }*/


        /*public Result GetLog()
        {
            Task<Result> task = Task.Run<Result>(async () => await GetLogAsync());
            return task.Result;
        }

        public async Task<Result> GetLogAsync()
        {
            var result = await _logger.GetAsync();
            // more code here...
            return result as Result;
        }*/


        /*public async Task DoWorker(int timeOut, int workerThreadID, ManualResetEvent _tme)
        {
            var tcs = new TaskCompletionSource<object>();
            var thread = new Thread(() =>
            {
                if (queueOfFunctions != null)
                {
                    if (queueOfFunctions.Count > 0)
                    {
                        hasPopped = queueOfFunctions.TryPop(out functionToCall);
                        var shaderChunk = functionToCall.chunkShader;
                        shaderChunk.Render(functionToCall);
                    }
                }
                /*try
                {

                            tcs.SetResult(null);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            await Task.Delay(1);
            //return tcs.Task;
        }*/
        private static readonly TaskFactory _taskFactory = new
                TaskFactory(CancellationToken.None,
                TaskCreationOptions.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default);

        public async Task DoWork(int timeOut, int workerThreadID, ManualResetEvent _tme)
        {
        _threadLoop:

            hasWorked = 0;
            if (queueOfFunctions != null)
            {
                if (queueOfFunctions.Count > 0)
                {
                    if (someSwitch == 1)
                    {
                        if (startPop == 1)
                        {


                            hasPopped = queueOfFunctions.TryPop(out functionToCall);
                            var shaderChunk = functionToCall.chunkShader;

                            var formatDelegate0 = new Action(delegate
                            {
                                shaderChunk.Renderer(functionToCall);
                            });

                            resultPending = formatDelegate0.BeginInvoke(null, null);
                            formatDelegate0.EndInvoke(resultPending);




                            /*hasPopped = queueOfFunctions.TryPop(out functionToCall);
                            var shaderChunk = functionToCall.chunkShader;

                            Func<int> formatDelegate = () =>
                            {
                                shaderChunk.Renderer(functionToCall);
                                return 1;
                            };

                            var t2 = new Task<int>(formatDelegate);
                            await Task.Delay(1);
                            t2.RunSynchronously();
                            await Task.Delay(1);
                            t2.Dispose();
                            await Task.Delay(1);*/
                            /*var formatDelegate0 = new Action(delegate
                            {
                                shaderChunk.Renderer(functionToCall);
                                //await Task.Delay(1);
                            });
                            */




                            //var t = _taskFactory.StartNew(formatDelegate0);//.Unwrap().GetAwaiter().GetResult();
                            //t.RunSynchronously();
                            //Task.WaitAll(t);
                            //t.Dispose();



                            //AsyncUtil.RunSync(() => shaderChunk.Renderer(functionToCall));


                            //var t = shaderChunk.Render(functionToCall);B

                            //t.RunSynchronously();

                            //Task.WaitAny(TaskExtensions.CompletedTask);
                            //Task.WaitAll(t);

                            //t.RunSynchronously();
                            //Task.WaitAll(t);
                            //t.Dispose();
                            //Console.WriteLine("test");




                            //await Task.Delay(1000);
                            //await Task.Delay(1000).ConfigureAwait(continueOnCapturedContext: true);

















                            /*var refreshDXEngineAction = new Action(delegate
                            {
                                hasPopped = queueOfFunctions.TryPop(out functionToCall);
                                var shaderChunk = functionToCall.chunkShader;

                                var formatDelegate0 = new Action(delegate
                                {
                                    shaderChunk.Render(functionToCall);
                                    //await Task.Delay(1);
                                });
                                formatDelegate0.Invoke();// (null, null);
                                                         //formatDelegate0.EndInvoke(resultPending);
                            });
                            SC_Console_APP.Program.MainDispatch.Invoke(System.Windows.Threading.DispatcherPriority.Normal, refreshDXEngineAction);
                            */


                            /*Func<bool> formatDelegate = () => 
                            {
                                shaderChunk.Render(functionToCall);
                                return true;
                            };*/

                            //var t2 = new Task<bool>(formatDelegate);
                            //t2.RunSynchronously();

                            //shaderChunk.Render(functionToCall);
                            //shaderChunk.Render(functionToCall)

                            //formatDelegate.BeginInvoke(null,null)

                            //Task<Result> task = Task.Run<Result>(async () => await GetLogAsync());

                            //var result = await shaderChunk.Render(functionToCall);//formatDelegate.BeginInvoke(null,null);






                            /*var refreshDXEngineAction = new Action(delegate
                            {



                                var formatDelegate0 = new Action(delegate
                                {
                                    shaderChunk.Render(functionToCall);
                                    //await Task.Delay(1);
                                });
                                resultPending = formatDelegate0.BeginInvoke(null, null);
                                formatDelegate0.EndInvoke(resultPending);

                            });
                            //Task.WaitAll();
                            SC_Console_APP.Program.MainDispatch.Invoke(System.Windows.Threading.DispatcherPriority.Normal, refreshDXEngineAction);
                            //Task.WaitAll();*/



                            /*hasPopped = queueOfFunctions.TryPop(out functionToCall);
                            var shaderChunk = functionToCall.chunkShader;
                            shaderChunk.Render(functionToCall);
                            try
                            {
                                Task reportTask = Task.Factory.StartNew(
                                    () =>
                                    {
                                        var refreshDXEngineAction = new Action(delegate
                                        {
                                            var formatDelegate0 = new Action(delegate
                                            {
                                                shaderChunk.Render(functionToCall);
                                                //await Task.Delay(1);
                                            });
                                            resultPending = formatDelegate0.BeginInvoke(null, null);
                                            formatDelegate0.EndInvoke(resultPending);

                                        });
                                        //Task.WaitAll();
                                        SC_Console_APP.Program.MainDispatch.Invoke(System.Windows.Threading.DispatcherPriority.Normal, refreshDXEngineAction);
                                        //shaderChunk.Render(functionToCall);
                                    }
                                                , CancellationToken.None
                                                , TaskCreationOptions.None
                                                , TaskScheduler.FromCurrentSynchronizationContext()
                                                );

                                reportTask.Wait();
                            }
                            catch
                            {

                            }*/




                            //Console.WriteLine("test");


                            /*Task<Result> task = Task.Run<Result>(async () => await shaderChunk.Render(functionToCall));

                            //formatDelegate0.Invoke();*/
                            //formatDelegate0.EndInvoke(resultPending);


                            //var formatDelegate0 = new Action();
                            //shaderChunk.Render(functionToCall);



                            //resultPending = formatDelegate0.BeginInvoke(null, null);
                            //formatDelegate0.EndInvoke(resultPending);

                            startPop = 0;
                        }

                        /*var shaderManager = functionToCall.shaderManager;
                        var formatDelegate0 = new Action(async delegate
                        {
                            shaderManager.RenderChunkShader(functionToCall);
                            await Task.Delay(1);
                        });

                        resultPending = formatDelegate0.BeginInvoke(null, null);
                        formatDelegate0.EndInvoke(resultPending);
                        */




                        /*MapThreadInfo<IAsyncResult> test = new MapThreadInfo<IAsyncResult>();
                        test.callback = checkToken;
                        test.parameter = 
                        var result = formatDelegate0.EndInvoke(resultPending);
                        */



                        //MapThreadInfo<IAsyncResult> test = new MapThreadInfo<IAsyncResult>();
                        //test.callback = checkToken;
                        //test.parameter = functionToCall.BeginInvoke(null, null);


                        //AsyncMethodCaller tester = new AsyncMethodCaller(testMethod);
                        //IAsyncResult resulter = tester.BeginInvoke(1, out threadId, new AsyncCallback(checkToken), null);



                        /*if (hasPopped)
                        {
                            //var t2 = new Task<int>(functionToCall);
                            //t2.RunSynchronously();
                        }

                        //var obj = resultPending.AsyncState;
                        */

                        startPop = 1;


                        /*if (resultPending.IsCompleted)
                        {

                            hasWorked = 1;
                            startPop = 1;
                            //Console.WriteLine("win");
                        }
                        else
                        {
                            //Console.WriteLine("fail");
                        }*/
                    }
                }
            }
            //Console.WriteLine("fail");
            //startPop = 1;
            await Task.Delay(timeOut);
            Thread.Sleep(1);
            //await Task.Yield();
            //_tme.WaitOne();
            goto _threadLoop;
        }

        public delegate int AsyncMethodCaller(int callDuration, out int threadId);
        public static int testMethod(int callDuration, out int threadId)
        {
            //resultPending = functionToCall.BeginInvoke(new AsyncCallback(checkToken), null);
            //var result = functionToCall.EndInvoke(resultPending);

            threadId = 0;
            return threadId;
        }

        public static class TaskExtensions
        {
            public static readonly Task CompletedTask = Task.FromResult(false);
        }


        public static void checkToken(IAsyncResult ar)
        {

        }


        SC_ThreadPool threadPool;



        public int currentStatus = 0;
        public int _workerThreadID;
        public int jobResult = -1;





    }
    public class SC_ThreadPool
    {
        public static SC_ThreadPool threadPool;
        ManualResetEvent[] _syncEvent;
        //WorkerThread[] _listOfThreads;
        List<WorkerThread> _listOfThreads = new List<WorkerThread>();
        //IAsyncResult
        public int _workerThreads = 0;
        public ConcurrentStack<SC_DirectX.chunkData> queueOfFunctions = new ConcurrentStack<SC_DirectX.chunkData>();
        //public SharpDX.Direct3D11.Device _device;

        //System.Func<int>

        public SC_ThreadPool() //int workerThreads
        {
            threadPool = this;
        }


        public void startPool(int workerThreads) //,SharpDX.Direct3D11.Device device
        {
            //Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            //this._device = device;
            this._workerThreads = workerThreads;
            _syncEvent = new ManualResetEvent[_workerThreads];
            //_listOfThreads = new WorkerThread[_workerThreads];
            for (int i = 0; i < _workerThreads; i++)
            {
                _syncEvent[i] = new ManualResetEvent(false);
                //_syncEvent[i].Set();

                WorkerThread workThread = new WorkerThread(i, _syncEvent[i]);
                _listOfThreads.Add(workThread);
            }

            //Thread threadDispatch = new Thread(() => Monitor(_syncEvent, _listOfThreads));
            //threadDispatch.IsBackground = true;
            //threadDispatch.Start();
        }
        public static SC_DirectX.chunkData functionToCall;
        public void Monitor(ManualResetEvent[] threadReset, List<WorkerThread> listOfThreads)
        {
            int counterOfThreadsNotStarted = 0;
            int switchForStartingOperations = 1;
            int counterOfThreadsTasking = 0;
            //int[] arrayOfThreadIDs = new int[threadReset.Length];
            List<int> arrayOfThreadIDs = new List<int>();
        //Func<int>
        _threadLoop:





            //arrayOfThreadIDs.Clear();

            /*if (switchForStartingOperations == 1)
            {
                for (int i = 0; i < listOfThreads.Count; i++)
                {
                    if (listOfThreads[i].currentStatus == 0)
                    {
                        //listOfThreads[i].startThread();
                        arrayOfThreadIDs.Add(i);
                        //thread isnt started yet.
                        //counterOfThreadsNotStarted++;   
                        //listOfThreads[i]._TME.Set();
                        //listOfThreads[i].

                        //
                    }
                }
                switchForStartingOperations = 0;
            }*/


            if (queueOfFunctions.Count > 0)
            {
                /*var refreshDXEngineAction = new Action(delegate
                {

                    bool hasPopped = queueOfFunctions.TryPop(out functionToCall);
                    var shaderChunk = functionToCall.chunkShader;

                    var formatDelegate0 = new Action(delegate
                    {
                        shaderChunk.Render(functionToCall);
                        //await Task.Delay(1);
                    });
                    formatDelegate0.Invoke();// (null, null);
                                             //formatDelegate0.EndInvoke(resultPending);
                });
                SC_Console_APP.Program.MainDispatch.Invoke(System.Windows.Threading.DispatcherPriority.Normal, refreshDXEngineAction);*/


                for (int i = 0; i < listOfThreads.Count; i++)
                {
                    /*if (listOfThreads[i].tsk!= null)
                    {
                        Console.WriteLine(listOfThreads[i].tsk.IsCompleted);
                    }*/
                    //awaiter.GetResult();
                }
                /*if (arrayOfThreadIDs.Count > 0)
                {
                    for (int i = arrayOfThreadIDs.Count - 1; i >= 0; i--)
                    {
                        listOfThreads[arrayOfThreadIDs[i]]._TME.Set();
                        arrayOfThreadIDs.Remove(arrayOfThreadIDs[i]);
                    }
                }
                else*/
                {
                    /*for (int i = listOfThreads.Count - 1; i >= 0; i--)
                    {
                        if (listOfThreads[i].currentStatus == 0)
                        {
                            if (listOfThreads[i].hasWorked == 1)
                            {
                                arrayOfThreadIDs.Add(i);
                            }
                        }
                    }*/
                }
                Thread.Sleep(1);
            }
            else
            {
                Thread.Sleep(100);
            }

            goto _threadLoop;
        }




        public object dummyObject = new object();
        public void AddToQueue(SC_DirectX.chunkData functionToAdd)
        {
            queueOfFunctions.Push(functionToAdd);
        }


        public void print()
        {
            Console.WriteLine(queueOfFunctions.Count);
        }



    }
}
