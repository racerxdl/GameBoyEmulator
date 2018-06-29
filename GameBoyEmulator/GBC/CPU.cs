﻿using System;
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
        internal bool _halt;
        internal GPU gpu;
        internal bool running;
        internal bool paused;
        internal bool step;

        internal Thread cpuThread;

        internal Mutex mtx;
        
        public delegate void PauseEvent();

        public event PauseEvent OnPause;
        
        public CPU() {
            reg = new CPURegisters();
            memory = new Memory(this);
            gpu = new GPU(this);
            mtx = new Mutex();
            Reset();
            LastUpdate = DateTime.Now;
            cpuThread = new Thread(() => Update());
            cpuThread.IsBackground = true;
            running = false;
            paused = true;
            step = false;
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
            mtx.ReleaseMutex();
        }

        public void Update() {
            Console.WriteLine("CPU Update Thread Started");
            while (running) {
                if (!_halt && !paused) {
                    var delta = DateTime.Now - LastUpdate;
                    if (!(delta.TotalMilliseconds > CPU_PERIOD_MS)) continue;
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
            reg.CycleCount++;
            var pc = reg.PC;
            reg.PC++;
            var op = memory.ReadByte(pc);
            opcodes[op](this);
            clockM += reg.lastClockM;
            clockT += reg.lastClockT;
            
            gpu.Cycle();
            mtx.ReleaseMutex();
        }

        #region CPU Instructions
        private static readonly List<Action<CPU>> opcodes = new List<Action<CPU>> {
            #region 0x00 Group
            (cpu) => CPUInstructions.NOP(cpu),
            (cpu) => CPUInstructions.LD__nn(cpu, "B", "C"),
            (cpu) => CPUInstructions.LD__m_(cpu, "B", "C", "A"),
            (cpu) => CPUInstructions.INC(cpu, "B", "C"),
            (cpu) => CPUInstructions.INCr(cpu, "B"),
            (cpu) => CPUInstructions.DECr(cpu, "B"),
            (cpu) => CPUInstructions.LDrn_(cpu, "B"),
            (cpu) => CPUInstructions.RLCA(cpu),
            (cpu) => CPUInstructions.LDmmSP(cpu),
            (cpu) => CPUInstructions.ADDHL(cpu, "B", "C"),
            (cpu) => CPUInstructions.LD___m(cpu, "A", "B", "C"),
            (cpu) => CPUInstructions.DEC(cpu, "B", "C"),
            (cpu) => CPUInstructions.INCr(cpu, "C"),
            (cpu) => CPUInstructions.DECr(cpu, "C"),
            (cpu) => CPUInstructions.LDrn_(cpu, "C"),
            (cpu) => CPUInstructions.RRCA(cpu),
            #endregion
            #region 0x10 Group
            (cpu) => CPUInstructions.DJNZn(cpu),
            (cpu) => CPUInstructions.LD__nn(cpu, "D", "E"),
            (cpu) => CPUInstructions.LD__m_(cpu, "D", "E", "A"),
            (cpu) => CPUInstructions.INC(cpu, "D", "E"),
            (cpu) => CPUInstructions.INCr(cpu, "D"),
            (cpu) => CPUInstructions.DECr(cpu, "D"),
            (cpu) => CPUInstructions.LDrn_(cpu, "D"),
            (cpu) => CPUInstructions.RLA(cpu),
            (cpu) => CPUInstructions.JRn(cpu),
            (cpu) => CPUInstructions.ADDHL(cpu, "D", "E"),
            (cpu) => CPUInstructions.LD___m(cpu, "A", "D", "E"),
            (cpu) => CPUInstructions.DEC(cpu, "D", "E"),
            (cpu) => CPUInstructions.INCr(cpu, "E"),
            (cpu) => CPUInstructions.DECr(cpu, "E"),
            (cpu) => CPUInstructions.LDrn_(cpu, "E"),
            (cpu) => CPUInstructions.RRA(cpu),
            #endregion
            #region 0x20 Group
            (cpu) => CPUInstructions.JRNZn(cpu),
            (cpu) => CPUInstructions.LD__nn(cpu, "H", "L"),
            (cpu) => CPUInstructions.LDHLIA(cpu),
            (cpu) => CPUInstructions.INCHL(cpu),
            (cpu) => CPUInstructions.INCr(cpu, "H"),
            (cpu) => CPUInstructions.DECr(cpu, "H"),
            (cpu) => CPUInstructions.LDrn_(cpu, "H"),
            (cpu) => CPUInstructions.DAA(cpu),
            (cpu) => CPUInstructions.JRZn(cpu),
            (cpu) => CPUInstructions.ADDHL(cpu, "H", "L"),
            (cpu) => CPUInstructions.LDAHLI(cpu),
            (cpu) => CPUInstructions.DECHL(cpu),
            (cpu) => CPUInstructions.INCr(cpu, "L"),
            (cpu) => CPUInstructions.DECr(cpu, "L"),
            (cpu) => CPUInstructions.LDrn_(cpu, "L"),
            (cpu) => CPUInstructions.CPL(cpu),
            #endregion
            #region 0x30 Group
            (cpu) => CPUInstructions.JRNCn(cpu),
            (cpu) => CPUInstructions.LDSPnn(cpu),
            (cpu) => CPUInstructions.LDHLDA(cpu),
            (cpu) => CPUInstructions.INCSP(cpu),
            (cpu) => CPUInstructions.INCHLm(cpu),
            (cpu) => CPUInstructions.DECHLm(cpu),
            (cpu) => CPUInstructions.LDHLmn(cpu),
            (cpu) => CPUInstructions.SCF(cpu),
            (cpu) => CPUInstructions.JRCn(cpu),
            (cpu) => CPUInstructions.ADDHLSP(cpu),
            (cpu) => CPUInstructions.LDAHLD(cpu),
            (cpu) => CPUInstructions.DECSP(cpu),
            (cpu) => CPUInstructions.INCr(cpu, "A"),
            (cpu) => CPUInstructions.DECr(cpu, "A"),
            (cpu) => CPUInstructions.LDrn_(cpu, "A"),
            (cpu) => CPUInstructions.CCF(cpu),
            #endregion
            #region 0x40 Group
            (cpu) => CPUInstructions.LDrr(cpu, "B", "B"),
            (cpu) => CPUInstructions.LDrr(cpu, "B", "C"),
            (cpu) => CPUInstructions.LDrr(cpu, "B", "D"),
            (cpu) => CPUInstructions.LDrr(cpu, "B", "E"),
            (cpu) => CPUInstructions.LDrr(cpu, "B", "H"),
            (cpu) => CPUInstructions.LDrr(cpu, "B", "L"),
            (cpu) => CPUInstructions.LDrHLm_(cpu, "B"),
            (cpu) => CPUInstructions.LDrr(cpu, "B", "A"),
            (cpu) => CPUInstructions.LDrr(cpu, "C", "B"),
            (cpu) => CPUInstructions.LDrr(cpu, "C", "C"),
            (cpu) => CPUInstructions.LDrr(cpu, "C", "D"),
            (cpu) => CPUInstructions.LDrr(cpu, "C", "E"),
            (cpu) => CPUInstructions.LDrr(cpu, "C", "H"),
            (cpu) => CPUInstructions.LDrr(cpu, "C", "L"),
            (cpu) => CPUInstructions.LDrHLm_(cpu, "C"),
            (cpu) => CPUInstructions.LDrr(cpu, "C", "A"),
            #endregion
            #region 0x50 Group
            (cpu) => CPUInstructions.LDrr(cpu, "D", "B"),
            (cpu) => CPUInstructions.LDrr(cpu, "D", "C"),
            (cpu) => CPUInstructions.LDrr(cpu, "D", "D"),
            (cpu) => CPUInstructions.LDrr(cpu, "D", "E"),
            (cpu) => CPUInstructions.LDrr(cpu, "D", "H"),
            (cpu) => CPUInstructions.LDrr(cpu, "D", "L"),
            (cpu) => CPUInstructions.LDrHLm_(cpu, "D"),
            (cpu) => CPUInstructions.LDrr(cpu, "D", "A"),
            (cpu) => CPUInstructions.LDrr(cpu, "E", "B"),
            (cpu) => CPUInstructions.LDrr(cpu, "E", "C"),
            (cpu) => CPUInstructions.LDrr(cpu, "E", "D"),
            (cpu) => CPUInstructions.LDrr(cpu, "E", "E"),
            (cpu) => CPUInstructions.LDrr(cpu, "E", "H"),
            (cpu) => CPUInstructions.LDrr(cpu, "E", "L"),
            (cpu) => CPUInstructions.LDrHLm_(cpu, "E"),
            (cpu) => CPUInstructions.LDrr(cpu, "E", "A"),
            #endregion
            #region 0x60 Group
            (cpu) => CPUInstructions.LDrr(cpu, "H", "B"),
            (cpu) => CPUInstructions.LDrr(cpu, "H", "C"),
            (cpu) => CPUInstructions.LDrr(cpu, "H", "D"),
            (cpu) => CPUInstructions.LDrr(cpu, "H", "E"),
            (cpu) => CPUInstructions.LDrr(cpu, "H", "H"),
            (cpu) => CPUInstructions.LDrr(cpu, "H", "L"),
            (cpu) => CPUInstructions.LDrHLm_(cpu, "H"),
            (cpu) => CPUInstructions.LDrr(cpu, "H", "A"),
            (cpu) => CPUInstructions.LDrr(cpu, "L", "B"),
            (cpu) => CPUInstructions.LDrr(cpu, "L", "C"),
            (cpu) => CPUInstructions.LDrr(cpu, "L", "D"),
            (cpu) => CPUInstructions.LDrr(cpu, "L", "E"),
            (cpu) => CPUInstructions.LDrr(cpu, "L", "H"),
            (cpu) => CPUInstructions.LDrr(cpu, "L", "L"),
            (cpu) => CPUInstructions.LDrHLm_(cpu, "L"),
            (cpu) => CPUInstructions.LDrr(cpu, "L", "A"),
            #endregion
            #region 0x70 Group
            (cpu) => CPUInstructions.LDHLmr_(cpu, "B"),
            (cpu) => CPUInstructions.LDHLmr_(cpu, "C"),
            (cpu) => CPUInstructions.LDHLmr_(cpu, "D"),
            (cpu) => CPUInstructions.LDHLmr_(cpu, "E"),
            (cpu) => CPUInstructions.LDHLmr_(cpu, "H"),
            (cpu) => CPUInstructions.LDHLmr_(cpu, "L"),
            (cpu) => CPUInstructions.HALT(cpu),
            (cpu) => CPUInstructions.LDHLmr_(cpu, "A"),
            (cpu) => CPUInstructions.LDrr(cpu, "A", "B"),
            (cpu) => CPUInstructions.LDrr(cpu, "A", "C"),
            (cpu) => CPUInstructions.LDrr(cpu, "A", "D"),
            (cpu) => CPUInstructions.LDrr(cpu, "A", "E"),
            (cpu) => CPUInstructions.LDrr(cpu, "A", "H"),
            (cpu) => CPUInstructions.LDrr(cpu, "A", "L"),
            (cpu) => CPUInstructions.LDrHLm_(cpu, "A"),
            (cpu) => CPUInstructions.LDrr(cpu, "A", "A"),
            #endregion
            #region 0x80 Group
            (cpu) => CPUInstructions.ADDr(cpu, "B"),
            (cpu) => CPUInstructions.ADDr(cpu, "C"),
            (cpu) => CPUInstructions.ADDr(cpu, "D"),
            (cpu) => CPUInstructions.ADDr(cpu, "E"),
            (cpu) => CPUInstructions.ADDr(cpu, "H"),
            (cpu) => CPUInstructions.ADDr(cpu, "L"),
            (cpu) => CPUInstructions.ADDHL(cpu),
            (cpu) => CPUInstructions.ADDr(cpu, "A"),
            (cpu) => CPUInstructions.ADCr(cpu, "B"),
            (cpu) => CPUInstructions.ADCr(cpu, "C"),
            (cpu) => CPUInstructions.ADCr(cpu, "D"),
            (cpu) => CPUInstructions.ADCr(cpu, "E"),
            (cpu) => CPUInstructions.ADCr(cpu, "H"),
            (cpu) => CPUInstructions.ADCr(cpu, "L"),
            (cpu) => CPUInstructions.ADCHL(cpu),
            (cpu) => CPUInstructions.ADCr(cpu, "A"),
            #endregion
            #region 0x90 Group
            (cpu) => CPUInstructions.SUBr(cpu, "B"),
            (cpu) => CPUInstructions.SUBr(cpu, "C"),
            (cpu) => CPUInstructions.SUBr(cpu, "D"),
            (cpu) => CPUInstructions.SUBr(cpu, "E"),
            (cpu) => CPUInstructions.SUBr(cpu, "H"),
            (cpu) => CPUInstructions.SUBr(cpu, "L"),
            (cpu) => CPUInstructions.SUBHL(cpu),
            (cpu) => CPUInstructions.SUBr(cpu, "A"),
            (cpu) => CPUInstructions.SBCr(cpu, "B"),
            (cpu) => CPUInstructions.SBCr(cpu, "C"),
            (cpu) => CPUInstructions.SBCr(cpu, "D"),
            (cpu) => CPUInstructions.SBCr(cpu, "E"),
            (cpu) => CPUInstructions.SBCr(cpu, "H"),
            (cpu) => CPUInstructions.SBCr(cpu, "L"),
            (cpu) => CPUInstructions.SBCHL(cpu),
            (cpu) => CPUInstructions.SBCr(cpu, "A"),
            #endregion
            #region 0xA0 Group
            (cpu) => CPUInstructions.ANDr(cpu, "B"),
            (cpu) => CPUInstructions.ANDr(cpu, "C"),
            (cpu) => CPUInstructions.ANDr(cpu, "D"),
            (cpu) => CPUInstructions.ANDr(cpu, "E"),
            (cpu) => CPUInstructions.ANDr(cpu, "H"),
            (cpu) => CPUInstructions.ANDr(cpu, "L"),
            (cpu) => CPUInstructions.ANDHL(cpu),
            (cpu) => CPUInstructions.ANDr(cpu, "A"),
            (cpu) => CPUInstructions.XORr(cpu, "B"),
            (cpu) => CPUInstructions.XORr(cpu, "C"),
            (cpu) => CPUInstructions.XORr(cpu, "D"),
            (cpu) => CPUInstructions.XORr(cpu, "E"),
            (cpu) => CPUInstructions.XORr(cpu, "H"),
            (cpu) => CPUInstructions.XORr(cpu, "L"),
            (cpu) => CPUInstructions.XORHL(cpu),
            (cpu) => CPUInstructions.XORr(cpu, "A"),
            #endregion
            #region 0xB0 Group
            (cpu) => CPUInstructions.ORr(cpu, "B"),
            (cpu) => CPUInstructions.ORr(cpu, "C"),
            (cpu) => CPUInstructions.ORr(cpu, "D"),
            (cpu) => CPUInstructions.ORr(cpu, "E"),
            (cpu) => CPUInstructions.ORr(cpu, "H"),
            (cpu) => CPUInstructions.ORr(cpu, "L"),
            (cpu) => CPUInstructions.ORHL(cpu),
            (cpu) => CPUInstructions.ORr(cpu, "A"),
            (cpu) => CPUInstructions.CPr(cpu, "B"),
            (cpu) => CPUInstructions.CPr(cpu, "C"),
            (cpu) => CPUInstructions.CPr(cpu, "D"),
            (cpu) => CPUInstructions.CPr(cpu, "E"),
            (cpu) => CPUInstructions.CPr(cpu, "H"),
            (cpu) => CPUInstructions.CPr(cpu, "L"),
            (cpu) => CPUInstructions.CPHL(cpu),
            (cpu) => CPUInstructions.CPr(cpu, "A"),
            #endregion
            #region 0xC0 Group
            (cpu) => CPUInstructions.RETNZ(cpu),
            (cpu) => CPUInstructions.POP(cpu, "B", "C"),
            (cpu) => CPUInstructions.JPNZnn(cpu),
            (cpu) => CPUInstructions.JPnn(cpu),
            (cpu) => CPUInstructions.CALLNZnn(cpu),
            (cpu) => CPUInstructions.PUSH(cpu, "B", "C"),
            (cpu) => CPUInstructions.ADDn(cpu),
            (cpu) => CPUInstructions.RSTXX(cpu, 0x00),
            (cpu) => CPUInstructions.RETZ(cpu),
            (cpu) => CPUInstructions.RET(cpu),
            (cpu) => CPUInstructions.JPZnn(cpu),
            (cpu) => CPUInstructions.CBCall(cpu),
            (cpu) => CPUInstructions.CALLZnn(cpu),
            (cpu) => CPUInstructions.CALLnn(cpu),
            (cpu) => CPUInstructions.ADCn(cpu),
            (cpu) => CPUInstructions.RSTXX(cpu, 0x08),
            #endregion
            #region 0xD0 Group
            (cpu) => CPUInstructions.RETNC(cpu),
            (cpu) => CPUInstructions.POP(cpu, "D", "E"),
            (cpu) => CPUInstructions.JPNCnn(cpu),
            (cpu) => CPUInstructions.NOPWARN(cpu, 0xD3),
            (cpu) => CPUInstructions.CALLNCnn(cpu),
            (cpu) => CPUInstructions.PUSH(cpu, "D", "E"),
            (cpu) => CPUInstructions.SUBn(cpu),
            (cpu) => CPUInstructions.RSTXX(cpu, 0x10),
            (cpu) => CPUInstructions.RETC(cpu),
            (cpu) => CPUInstructions.RETI(cpu),
            (cpu) => CPUInstructions.JPCnn(cpu),
            (cpu) => CPUInstructions.NOPWARN(cpu, 0xDB),
            (cpu) => CPUInstructions.CALLCnn(cpu),
            (cpu) => CPUInstructions.NOPWARN(cpu, 0xDD),
            (cpu) => CPUInstructions.SBCn(cpu),
            (cpu) => CPUInstructions.RSTXX(cpu, 0x18),
            #endregion
            #region 0xE0 Group
            (cpu) => CPUInstructions.LDIOnA(cpu),
            (cpu) => CPUInstructions.POP(cpu, "H", "L"),
            (cpu) => CPUInstructions.LDIOCA(cpu),
            (cpu) => CPUInstructions.NOPWARN(cpu, 0xE3),
            (cpu) => CPUInstructions.NOPWARN(cpu, 0xE4),
            (cpu) => CPUInstructions.PUSH(cpu, "H", "L"),
            (cpu) => CPUInstructions.ANDn(cpu),
            (cpu) => CPUInstructions.RSTXX(cpu, 0x20),
            (cpu) => CPUInstructions.ADDSPn(cpu),
            (cpu) => CPUInstructions.JPHL(cpu),
            (cpu) => CPUInstructions.LDmm_(cpu, "A"),
            (cpu) => CPUInstructions.NOPWARN(cpu, 0xEB),
            (cpu) => CPUInstructions.NOPWARN(cpu, 0xEC),
            (cpu) => CPUInstructions.NOPWARN(cpu, 0xED),
            (cpu) => CPUInstructions.XORn(cpu),
            (cpu) => CPUInstructions.RSTXX(cpu, 0x28),
            #endregion
            #region 0xF0 Group
            (cpu) => CPUInstructions.LDAIOn(cpu),
            (cpu) => CPUInstructions.POP(cpu, "A", "F"),
            (cpu) => CPUInstructions.LDAIOC(cpu),
            (cpu) => CPUInstructions.DI(cpu),
            (cpu) => CPUInstructions.NOPWARN(cpu, 0xF4),
            (cpu) => CPUInstructions.PUSH(cpu, "A", "F"),
            (cpu) => CPUInstructions.ORn(cpu),
            (cpu) => CPUInstructions.RSTXX(cpu, 0x30),
            (cpu) => CPUInstructions.LDHLSPn(cpu),
            (cpu) => CPUInstructions.LDHLSPr(cpu),
            (cpu) => CPUInstructions.LD_mm(cpu, "A"),
            (cpu) => CPUInstructions.EI(cpu),
            (cpu) => CPUInstructions.NOPWARN(cpu, 0xFC),
            (cpu) => CPUInstructions.NOPWARN(cpu, 0xFD),
            (cpu) => CPUInstructions.CPn(cpu),
            (cpu) => CPUInstructions.RSTXX(cpu, 0x38),
            #endregion
        };
        #endregion
    }
}
