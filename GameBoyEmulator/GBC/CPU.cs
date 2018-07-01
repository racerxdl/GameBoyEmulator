using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace GameBoyEmulator.Desktop.GBC {
    public class CPU {
        private const int CPU_CLOCK = 4194304;
        private const float CPU_PERIOD_MS = 1000f / CPU_CLOCK;

        private DateTime LastUpdate;
        
        internal int clockM;
        internal int clockT;
        internal CPURegisters reg;
        internal Memory memory;
        internal GBKeys GbKeys;
        internal GBTimer timer;
        internal bool _halt;
        internal GPU gpu;
        internal bool running;
        internal bool paused;
        internal bool step;

        internal Thread cpuThread;

        internal Mutex mtx;
        internal double lastCycleTimeMs;
        
        public delegate void PauseEvent();

        public event PauseEvent OnPause;
        
        public CPU() {
            reg = new CPURegisters();
            memory = new Memory(this);
            gpu = new GPU(this);
            mtx = new Mutex();
            GbKeys = new GBKeys(this);
            timer = new GBTimer(this);
            Reset();
            LastUpdate = DateTime.Now;
            cpuThread = new Thread(() => Update());
            cpuThread.IsBackground = true;
            running = false;
            paused = true;
            step = false;
            lastCycleTimeMs = 0;
        }

        public void Start() {
            if (!running) {
                Console.WriteLine("Starting");
                running = true;
                cpuThread.Start();
                Console.WriteLine("CPU Thread Started");
            }
        }

        public void Step() {
            mtx.WaitOne();
            paused = false;
            step = true;
            mtx.ReleaseMutex();
        }

        public void Continue() {
            mtx.WaitOne();
            paused = false;
            mtx.ReleaseMutex();
        }

        public void Pause() {
            mtx.WaitOne();
            paused = true;
            mtx.ReleaseMutex();
            OnPause?.Invoke();
        }
        
        public void Stop() {
            if (running) {
                Console.WriteLine("Stopping");
                running = false;
                cpuThread.Join();
            }
        }

        public void Reset() {
            mtx.WaitOne();
            _halt = false;
            clockT = 0;
            clockM = 0;
            reg.Reset();
            memory.Reset();
            gpu.Reset();
            GbKeys.Reset();
            timer.Reset();
            mtx.ReleaseMutex();
        }

        public void Update() {
            Console.WriteLine("CPU Update Thread Started");
            while (running) {
                if (!paused) {
                    var delta = DateTime.Now - LastUpdate;
                    //if (!(delta.TotalMilliseconds > CPU_PERIOD_MS)) continue;
                    lastCycleTimeMs = delta.TotalMilliseconds;
                    Cycle();
                    LastUpdate = DateTime.Now;
                    
                    if (!step) continue;
                    
                    step = false;
                    paused = true;
                    OnPause?.Invoke();
                } else {
                    Thread.Sleep(10);
                }
            }
            Console.WriteLine("CPU Update Thread Stopped");
        }

        public void Cycle() {
            mtx.WaitOne();
            // Normal Cycle
            reg.CycleCount++;
            var totalClockM = 0;
            var totalClockT = 0;
            if (_halt) {
                totalClockM += 1;
                totalClockT += 4;
            } else {
                var op = memory.ReadByte(reg.PC);
                reg.PC++;
                CPUInstructions.opcodes[op](this);
                totalClockM += reg.lastClockM;
                totalClockT += reg.lastClockT;
            }

            // Check Interrupts
            if (reg.InterruptEnable && reg.EnabledInterrupts != 0 && reg.TriggerInterrupts != 0) {
                _halt = false;
                reg.InterruptEnable = false;
                var interruptsFired = reg.EnabledInterrupts & reg.TriggerInterrupts;
                if ((interruptsFired & Flags.INT_VBLANK) > 0) {
                    reg.TriggerInterrupts &= (byte) ~Flags.INT_VBLANK;
                    CPUInstructions.RSTXX(this, Addresses.INT_VBLANK);  // V-Blank
                    totalClockM += reg.lastClockM;
                    totalClockT += reg.lastClockT;
                } else if ((interruptsFired & Flags.INT_LCDSTAT) > 0) {
                    reg.TriggerInterrupts &= (byte) ~Flags.INT_LCDSTAT;
                    CPUInstructions.RSTXX(this, Addresses.INT_LCDSTAT); // LCD Stat
                    totalClockM += reg.lastClockM;
                    totalClockT += reg.lastClockT;
                } else if ((interruptsFired & Flags.INT_TIMER) > 0) {
                    reg.TriggerInterrupts &= (byte) ~Flags.INT_TIMER;
                    CPUInstructions.RSTXX(this, Addresses.INT_TIMER);  // Timer
                    totalClockM += reg.lastClockM;
                    totalClockT += reg.lastClockT;
                } else if ((interruptsFired & Flags.INT_SERIAL) > 0) {
                    reg.TriggerInterrupts &= (byte) ~Flags.INT_SERIAL;
                    CPUInstructions.RSTXX(this, Addresses.INT_SERIAL); // Serial
                    totalClockM += reg.lastClockM;
                    totalClockT += reg.lastClockT;
                } else if ((interruptsFired & Flags.INT_JOYPAD) > 0) {
                    reg.TriggerInterrupts &= (byte) ~Flags.INT_JOYPAD;
                    CPUInstructions.RSTXX(this, Addresses.INT_JOYPAD); // Joypad Interrupt
                    totalClockM += reg.lastClockM;
                    totalClockT += reg.lastClockT;
                } else {
                    reg.InterruptEnable = true;
                }
            }
            
            clockM += totalClockM;
            clockT += totalClockT;
            
            // GPU
            gpu.Cycle();
            
            // Timers
            timer.Increment();
            
            mtx.ReleaseMutex();
        }
    }
}
