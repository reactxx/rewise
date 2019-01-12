﻿/*

Copyright (c) 2001, Dr Martin Porter
Copyright (c) 2002, Richard Boulton
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
    * this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright
    * notice, this list of conditions and the following disclaimer in the
    * documentation and/or other materials provided with the distribution.
    * Neither the name of the copyright holders nor the names of its contributors
    * may be used to endorse or promote products derived from this software
    * without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

 */

using System.Text;

namespace Lucene.Net.Tartarus.Snowball.Ext
{
    /// <summary>
    /// This class was automatically generated by a Snowball to Java compiler
    /// It implements the stemming algorithm defined by a snowball script.
    /// </summary>
    public class FinnishStemmer : SnowballProgram
    {
        private readonly static FinnishStemmer methodObject = new FinnishStemmer();

        private readonly static Among[] a_0 = {
                    new Among ( "pa", -1, 1, "", methodObject ),
                    new Among ( "sti", -1, 2, "", methodObject ),
                    new Among ( "kaan", -1, 1, "", methodObject ),
                    new Among ( "han", -1, 1, "", methodObject ),
                    new Among ( "kin", -1, 1, "", methodObject ),
                    new Among ( "h\u00E4n", -1, 1, "", methodObject ),
                    new Among ( "k\u00E4\u00E4n", -1, 1, "", methodObject ),
                    new Among ( "ko", -1, 1, "", methodObject ),
                    new Among ( "p\u00E4", -1, 1, "", methodObject ),
                    new Among ( "k\u00F6", -1, 1, "", methodObject )
                };

        private readonly static Among[] a_1 = {
                    new Among ( "lla", -1, -1, "", methodObject ),
                    new Among ( "na", -1, -1, "", methodObject ),
                    new Among ( "ssa", -1, -1, "", methodObject ),
                    new Among ( "ta", -1, -1, "", methodObject ),
                    new Among ( "lta", 3, -1, "", methodObject ),
                    new Among ( "sta", 3, -1, "", methodObject )
                };

        private readonly static Among[] a_2 = {
                    new Among ( "ll\u00E4", -1, -1, "", methodObject ),
                    new Among ( "n\u00E4", -1, -1, "", methodObject ),
                    new Among ( "ss\u00E4", -1, -1, "", methodObject ),
                    new Among ( "t\u00E4", -1, -1, "", methodObject ),
                    new Among ( "lt\u00E4", 3, -1, "", methodObject ),
                    new Among ( "st\u00E4", 3, -1, "", methodObject )
                };

        private readonly static Among[] a_3 = {
                    new Among ( "lle", -1, -1, "", methodObject ),
                    new Among ( "ine", -1, -1, "", methodObject )
                };

        private readonly static Among[] a_4 = {
                    new Among ( "nsa", -1, 3, "", methodObject ),
                    new Among ( "mme", -1, 3, "", methodObject ),
                    new Among ( "nne", -1, 3, "", methodObject ),
                    new Among ( "ni", -1, 2, "", methodObject ),
                    new Among ( "si", -1, 1, "", methodObject ),
                    new Among ( "an", -1, 4, "", methodObject ),
                    new Among ( "en", -1, 6, "", methodObject ),
                    new Among ( "\u00E4n", -1, 5, "", methodObject ),
                    new Among ( "ns\u00E4", -1, 3, "", methodObject )
                };

        private readonly static Among[] a_5 = {
                    new Among ( "aa", -1, -1, "", methodObject ),
                    new Among ( "ee", -1, -1, "", methodObject ),
                    new Among ( "ii", -1, -1, "", methodObject ),
                    new Among ( "oo", -1, -1, "", methodObject ),
                    new Among ( "uu", -1, -1, "", methodObject ),
                    new Among ( "\u00E4\u00E4", -1, -1, "", methodObject ),
                    new Among ( "\u00F6\u00F6", -1, -1, "", methodObject )
                };

        private readonly static Among[] a_6 = {
                    new Among ( "a", -1, 8, "", methodObject ),
                    new Among ( "lla", 0, -1, "", methodObject ),
                    new Among ( "na", 0, -1, "", methodObject ),
                    new Among ( "ssa", 0, -1, "", methodObject ),
                    new Among ( "ta", 0, -1, "", methodObject ),
                    new Among ( "lta", 4, -1, "", methodObject ),
                    new Among ( "sta", 4, -1, "", methodObject ),
                    new Among ( "tta", 4, 9, "", methodObject ),
                    new Among ( "lle", -1, -1, "", methodObject ),
                    new Among ( "ine", -1, -1, "", methodObject ),
                    new Among ( "ksi", -1, -1, "", methodObject ),
                    new Among ( "n", -1, 7, "", methodObject ),
                    new Among ( "han", 11, 1, "", methodObject ),
                    new Among ( "den", 11, -1, "r_VI", methodObject ),
                    new Among ( "seen", 11, -1, "r_LONG", methodObject ),
                    new Among ( "hen", 11, 2, "", methodObject ),
                    new Among ( "tten", 11, -1, "r_VI", methodObject ),
                    new Among ( "hin", 11, 3, "", methodObject ),
                    new Among ( "siin", 11, -1, "r_VI", methodObject ),
                    new Among ( "hon", 11, 4, "", methodObject ),
                    new Among ( "h\u00E4n", 11, 5, "", methodObject ),
                    new Among ( "h\u00F6n", 11, 6, "", methodObject ),
                    new Among ( "\u00E4", -1, 8, "", methodObject ),
                    new Among ( "ll\u00E4", 22, -1, "", methodObject ),
                    new Among ( "n\u00E4", 22, -1, "", methodObject ),
                    new Among ( "ss\u00E4", 22, -1, "", methodObject ),
                    new Among ( "t\u00E4", 22, -1, "", methodObject ),
                    new Among ( "lt\u00E4", 26, -1, "", methodObject ),
                    new Among ( "st\u00E4", 26, -1, "", methodObject ),
                    new Among ( "tt\u00E4", 26, 9, "", methodObject )
                };

        private readonly static Among[] a_7 = {
                    new Among ( "eja", -1, -1, "", methodObject ),
                    new Among ( "mma", -1, 1, "", methodObject ),
                    new Among ( "imma", 1, -1, "", methodObject ),
                    new Among ( "mpa", -1, 1, "", methodObject ),
                    new Among ( "impa", 3, -1, "", methodObject ),
                    new Among ( "mmi", -1, 1, "", methodObject ),
                    new Among ( "immi", 5, -1, "", methodObject ),
                    new Among ( "mpi", -1, 1, "", methodObject ),
                    new Among ( "impi", 7, -1, "", methodObject ),
                    new Among ( "ej\u00E4", -1, -1, "", methodObject ),
                    new Among ( "mm\u00E4", -1, 1, "", methodObject ),
                    new Among ( "imm\u00E4", 10, -1, "", methodObject ),
                    new Among ( "mp\u00E4", -1, 1, "", methodObject ),
                    new Among ( "imp\u00E4", 12, -1, "", methodObject )
                };

        private readonly static Among[] a_8 = {
                    new Among ( "i", -1, -1, "", methodObject ),
                    new Among ( "j", -1, -1, "", methodObject )
                };

        private readonly static Among[] a_9 = {
                    new Among ( "mma", -1, 1, "", methodObject ),
                    new Among ( "imma", 0, -1, "", methodObject )
                };

        private static readonly char[] g_AEI = { (char)17, (char)1, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)8 };

        private static readonly char[] g_V1 = { (char)17, (char)65, (char)16, (char)1, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)8, (char)0, (char)32 };

        private static readonly char[] g_V2 = { (char)17, (char)65, (char)16, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)8, (char)0, (char)32 };

        private static readonly char[] g_particle_end = { (char)17, (char)97, (char)24, (char)1, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)8, (char)0, (char)32 };

        private bool B_ending_removed;
        private StringBuilder S_x = new StringBuilder();
        private int I_p2;
        private int I_p1;

        private void copy_from(FinnishStemmer other)
        {
            B_ending_removed = other.B_ending_removed;
            S_x = other.S_x;
            I_p2 = other.I_p2;
            I_p1 = other.I_p1;
            base.CopyFrom(other);
        }

        private bool r_mark_regions()
        {
            int v_1;
            int v_3;
            // (, line 41
            I_p1 = m_limit;
            I_p2 = m_limit;
            // goto, line 46

            while (true)
            {
                v_1 = m_cursor;

                do
                {
                    if (!(InGrouping(g_V1, 97, 246)))
                    {
                        goto lab1;
                    }
                    m_cursor = v_1;
                    goto golab0;
                } while (false);
                lab1:
                m_cursor = v_1;
                if (m_cursor >= m_limit)
                {
                    return false;
                }
                m_cursor++;
            }
            golab0:
            // gopast, line 46

            while (true)
            {

                do
                {
                    if (!(OutGrouping(g_V1, 97, 246)))
                    {
                        goto lab3;
                    }
                    goto golab2;
                } while (false);
                lab3:
                if (m_cursor >= m_limit)
                {
                    return false;
                }
                m_cursor++;
            }
            golab2:
            // setmark p1, line 46
            I_p1 = m_cursor;
            // goto, line 47

            while (true)
            {
                v_3 = m_cursor;

                do
                {
                    if (!(InGrouping(g_V1, 97, 246)))
                    {
                        goto lab5;
                    }
                    m_cursor = v_3;
                    goto golab4;
                } while (false);
                lab5:
                m_cursor = v_3;
                if (m_cursor >= m_limit)
                {
                    return false;
                }
                m_cursor++;
            }
            golab4:
            // gopast, line 47

            while (true)
            {

                do
                {
                    if (!(OutGrouping(g_V1, 97, 246)))
                    {
                        goto lab7;
                    }
                    goto golab6;
                } while (false);
                lab7:
                if (m_cursor >= m_limit)
                {
                    return false;
                }
                m_cursor++;
            }
            golab6:
            // setmark p2, line 47
            I_p2 = m_cursor;
            return true;
        }

        private bool r_R2()
        {
            if (!(I_p2 <= m_cursor))
            {
                return false;
            }
            return true;
        }

        private bool r_particle_etc()
        {
            int among_var;
            int v_1;
            int v_2;
            // (, line 54
            // setlimit, line 55
            v_1 = m_limit - m_cursor;
            // tomark, line 55
            if (m_cursor < I_p1)
            {
                return false;
            }
            m_cursor = I_p1;
            v_2 = m_limit_backward;
            m_limit_backward = m_cursor;
            m_cursor = m_limit - v_1;
            // (, line 55
            // [, line 55
            m_ket = m_cursor;
            // substring, line 55
            among_var = FindAmongB(a_0, 10);
            if (among_var == 0)
            {
                m_limit_backward = v_2;
                return false;
            }
            // ], line 55
            m_bra = m_cursor;
            m_limit_backward = v_2;
            switch (among_var)
            {
                case 0:
                    return false;
                case 1:
                    // (, line 62
                    if (!(InGroupingB(g_particle_end, 97, 246)))
                    {
                        return false;
                    }
                    break;
                case 2:
                    // (, line 64
                    // call R2, line 64
                    if (!r_R2())
                    {
                        return false;
                    }
                    break;
            }
            // delete, line 66
            SliceDel();
            return true;
        }

        private bool r_possessive()
        {
            int among_var;
            int v_1;
            int v_2;
            int v_3;
            // (, line 68
            // setlimit, line 69
            v_1 = m_limit - m_cursor;
            // tomark, line 69
            if (m_cursor < I_p1)
            {
                return false;
            }
            m_cursor = I_p1;
            v_2 = m_limit_backward;
            m_limit_backward = m_cursor;
            m_cursor = m_limit - v_1;
            // (, line 69
            // [, line 69
            m_ket = m_cursor;
            // substring, line 69
            among_var = FindAmongB(a_4, 9);
            if (among_var == 0)
            {
                m_limit_backward = v_2;
                return false;
            }
            // ], line 69
            m_bra = m_cursor;
            m_limit_backward = v_2;
            switch (among_var)
            {
                case 0:
                    return false;
                case 1:
                    // (, line 72
                    // not, line 72
                    {
                        v_3 = m_limit - m_cursor;

                        do
                        {
                            // literal, line 72
                            if (!(Eq_S_B(1, "k")))
                            {
                                goto lab0;
                            }
                            return false;
                        } while (false);
                        lab0:
                        m_cursor = m_limit - v_3;
                    }
                    // delete, line 72
                    SliceDel();
                    break;
                case 2:
                    // (, line 74
                    // delete, line 74
                    SliceDel();
                    // [, line 74
                    m_ket = m_cursor;
                    // literal, line 74
                    if (!(Eq_S_B(3, "kse")))
                    {
                        return false;
                    }
                    // ], line 74
                    m_bra = m_cursor;
                    // <-, line 74
                    SliceFrom("ksi");
                    break;
                case 3:
                    // (, line 78
                    // delete, line 78
                    SliceDel();
                    break;
                case 4:
                    // (, line 81
                    // among, line 81
                    if (FindAmongB(a_1, 6) == 0)
                    {
                        return false;
                    }
                    // delete, line 81
                    SliceDel();
                    break;
                case 5:
                    // (, line 83
                    // among, line 83
                    if (FindAmongB(a_2, 6) == 0)
                    {
                        return false;
                    }
                    // delete, line 84
                    SliceDel();
                    break;
                case 6:
                    // (, line 86
                    // among, line 86
                    if (FindAmongB(a_3, 2) == 0)
                    {
                        return false;
                    }
                    // delete, line 86
                    SliceDel();
                    break;
            }
            return true;
        }

        private bool r_LONG()
        {
            // among, line 91
            if (FindAmongB(a_5, 7) == 0)
            {
                return false;
            }
            return true;
        }

        private bool r_VI()
        {
            // (, line 93
            // literal, line 93
            if (!(Eq_S_B(1, "i")))
            {
                return false;
            }
            if (!(InGroupingB(g_V2, 97, 246)))
            {
                return false;
            }
            return true;
        }

        private bool r_case_ending()
        {
            int among_var;
            int v_1;
            int v_2;
            int v_3;
            int v_4;
            int v_5;
            // (, line 95
            // setlimit, line 96
            v_1 = m_limit - m_cursor;
            // tomark, line 96
            if (m_cursor < I_p1)
            {
                return false;
            }
            m_cursor = I_p1;
            v_2 = m_limit_backward;
            m_limit_backward = m_cursor;
            m_cursor = m_limit - v_1;
            // (, line 96
            // [, line 96
            m_ket = m_cursor;
            // substring, line 96
            among_var = FindAmongB(a_6, 30);
            if (among_var == 0)
            {
                m_limit_backward = v_2;
                return false;
            }
            // ], line 96
            m_bra = m_cursor;
            m_limit_backward = v_2;
            switch (among_var)
            {
                case 0:
                    return false;
                case 1:
                    // (, line 98
                    // literal, line 98
                    if (!(Eq_S_B(1, "a")))
                    {
                        return false;
                    }
                    break;
                case 2:
                    // (, line 99
                    // literal, line 99
                    if (!(Eq_S_B(1, "e")))
                    {
                        return false;
                    }
                    break;
                case 3:
                    // (, line 100
                    // literal, line 100
                    if (!(Eq_S_B(1, "i")))
                    {
                        return false;
                    }
                    break;
                case 4:
                    // (, line 101
                    // literal, line 101
                    if (!(Eq_S_B(1, "o")))
                    {
                        return false;
                    }
                    break;
                case 5:
                    // (, line 102
                    // literal, line 102
                    if (!(Eq_S_B(1, "\u00E4")))
                    {
                        return false;
                    }
                    break;
                case 6:
                    // (, line 103
                    // literal, line 103
                    if (!(Eq_S_B(1, "\u00F6")))
                    {
                        return false;
                    }
                    break;
                case 7:
                    // (, line 111
                    // try, line 111
                    v_3 = m_limit - m_cursor;

                    do
                    {
                        // (, line 111
                        // and, line 113
                        v_4 = m_limit - m_cursor;
                        // or, line 112

                        do
                        {
                            v_5 = m_limit - m_cursor;

                            do
                            {
                                // call LONG, line 111
                                if (!r_LONG())
                                {
                                    goto lab2;
                                }
                                goto lab1;
                            } while (false);
                            lab2:
                            m_cursor = m_limit - v_5;
                            // literal, line 112
                            if (!(Eq_S_B(2, "ie")))
                            {
                                m_cursor = m_limit - v_3;
                                goto lab0;
                            }
                        } while (false);
                        lab1:
                        m_cursor = m_limit - v_4;
                        // next, line 113
                        if (m_cursor <= m_limit_backward)
                        {
                            m_cursor = m_limit - v_3;
                            goto lab0;
                        }
                        m_cursor--;
                        // ], line 113
                        m_bra = m_cursor;
                    } while (false);
                    lab0:
                    break;
                case 8:
                    // (, line 119
                    if (!(InGroupingB(g_V1, 97, 246)))
                    {
                        return false;
                    }
                    if (!(OutGroupingB(g_V1, 97, 246)))
                    {
                        return false;
                    }
                    break;
                case 9:
                    // (, line 121
                    // literal, line 121
                    if (!(Eq_S_B(1, "e")))
                    {
                        return false;
                    }
                    break;
            }
            // delete, line 138
            SliceDel();
            // set ending_removed, line 139
            B_ending_removed = true;
            return true;
        }

        private bool r_other_endings()
        {
            int among_var;
            int v_1;
            int v_2;
            int v_3;
            // (, line 141
            // setlimit, line 142
            v_1 = m_limit - m_cursor;
            // tomark, line 142
            if (m_cursor < I_p2)
            {
                return false;
            }
            m_cursor = I_p2;
            v_2 = m_limit_backward;
            m_limit_backward = m_cursor;
            m_cursor = m_limit - v_1;
            // (, line 142
            // [, line 142
            m_ket = m_cursor;
            // substring, line 142
            among_var = FindAmongB(a_7, 14);
            if (among_var == 0)
            {
                m_limit_backward = v_2;
                return false;
            }
            // ], line 142
            m_bra = m_cursor;
            m_limit_backward = v_2;
            switch (among_var)
            {
                case 0:
                    return false;
                case 1:
                    // (, line 146
                    // not, line 146
                    {
                        v_3 = m_limit - m_cursor;

                        do
                        {
                            // literal, line 146
                            if (!(Eq_S_B(2, "po")))
                            {
                                goto lab0;
                            }
                            return false;
                        } while (false);
                        lab0:
                        m_cursor = m_limit - v_3;
                    }
                    break;
            }
            // delete, line 151
            SliceDel();
            return true;
        }

        private bool r_i_plural()
        {
            int v_1;
            int v_2;
            // (, line 153
            // setlimit, line 154
            v_1 = m_limit - m_cursor;
            // tomark, line 154
            if (m_cursor < I_p1)
            {
                return false;
            }
            m_cursor = I_p1;
            v_2 = m_limit_backward;
            m_limit_backward = m_cursor;
            m_cursor = m_limit - v_1;
            // (, line 154
            // [, line 154
            m_ket = m_cursor;
            // substring, line 154
            if (FindAmongB(a_8, 2) == 0)
            {
                m_limit_backward = v_2;
                return false;
            }
            // ], line 154
            m_bra = m_cursor;
            m_limit_backward = v_2;
            // delete, line 158
            SliceDel();
            return true;
        }

        private bool r_t_plural()
        {
            int among_var;
            int v_1;
            int v_2;
            int v_3;
            int v_4;
            int v_5;
            int v_6;
            // (, line 160
            // setlimit, line 161
            v_1 = m_limit - m_cursor;
            // tomark, line 161
            if (m_cursor < I_p1)
            {
                return false;
            }
            m_cursor = I_p1;
            v_2 = m_limit_backward;
            m_limit_backward = m_cursor;
            m_cursor = m_limit - v_1;
            // (, line 161
            // [, line 162
            m_ket = m_cursor;
            // literal, line 162
            if (!(Eq_S_B(1, "t")))
            {
                m_limit_backward = v_2;
                return false;
            }
            // ], line 162
            m_bra = m_cursor;
            // test, line 162
            v_3 = m_limit - m_cursor;
            if (!(InGroupingB(g_V1, 97, 246)))
            {
                m_limit_backward = v_2;
                return false;
            }
            m_cursor = m_limit - v_3;
            // delete, line 163
            SliceDel();
            m_limit_backward = v_2;
            // setlimit, line 165
            v_4 = m_limit - m_cursor;
            // tomark, line 165
            if (m_cursor < I_p2)
            {
                return false;
            }
            m_cursor = I_p2;
            v_5 = m_limit_backward;
            m_limit_backward = m_cursor;
            m_cursor = m_limit - v_4;
            // (, line 165
            // [, line 165
            m_ket = m_cursor;
            // substring, line 165
            among_var = FindAmongB(a_9, 2);
            if (among_var == 0)
            {
                m_limit_backward = v_5;
                return false;
            }
            // ], line 165
            m_bra = m_cursor;
            m_limit_backward = v_5;
            switch (among_var)
            {
                case 0:
                    return false;
                case 1:
                    // (, line 167
                    // not, line 167
                    {
                        v_6 = m_limit - m_cursor;

                        do
                        {
                            // literal, line 167
                            if (!(Eq_S_B(2, "po")))
                            {
                                goto lab0;
                            }
                            return false;
                        } while (false);
                        lab0:
                        m_cursor = m_limit - v_6;
                    }
                    break;
            }
            // delete, line 170
            SliceDel();
            return true;
        }

        private bool r_tidy()
        {
            int v_1;
            int v_2;
            int v_3;
            int v_4;
            int v_5;
            int v_6;
            int v_7;
            int v_8;
            int v_9;
            // (, line 172
            // setlimit, line 173
            v_1 = m_limit - m_cursor;
            // tomark, line 173
            if (m_cursor < I_p1)
            {
                return false;
            }
            m_cursor = I_p1;
            v_2 = m_limit_backward;
            m_limit_backward = m_cursor;
            m_cursor = m_limit - v_1;
            // (, line 173
            // do, line 174
            v_3 = m_limit - m_cursor;

            do
            {
                // (, line 174
                // and, line 174
                v_4 = m_limit - m_cursor;
                // call LONG, line 174
                if (!r_LONG())
                {
                    goto lab0;
                }
                m_cursor = m_limit - v_4;
                // (, line 174
                // [, line 174
                m_ket = m_cursor;
                // next, line 174
                if (m_cursor <= m_limit_backward)
                {
                    goto lab0;
                }
                m_cursor--;
                // ], line 174
                m_bra = m_cursor;
                // delete, line 174
                SliceDel();
            } while (false);
            lab0:
            m_cursor = m_limit - v_3;
            // do, line 175
            v_5 = m_limit - m_cursor;

            do
            {
                // (, line 175
                // [, line 175
                m_ket = m_cursor;
                if (!(InGroupingB(g_AEI, 97, 228)))
                {
                    goto lab1;
                }
                // ], line 175
                m_bra = m_cursor;
                if (!(OutGroupingB(g_V1, 97, 246)))
                {
                    goto lab1;
                }
                // delete, line 175
                SliceDel();
            } while (false);
            lab1:
            m_cursor = m_limit - v_5;
            // do, line 176
            v_6 = m_limit - m_cursor;

            do
            {
                // (, line 176
                // [, line 176
                m_ket = m_cursor;
                // literal, line 176
                if (!(Eq_S_B(1, "j")))
                {
                    goto lab2;
                }
                // ], line 176
                m_bra = m_cursor;
                // or, line 176

                do
                {
                    v_7 = m_limit - m_cursor;

                    do
                    {
                        // literal, line 176
                        if (!(Eq_S_B(1, "o")))
                        {
                            goto lab4;
                        }
                        goto lab3;
                    } while (false);
                    lab4:
                    m_cursor = m_limit - v_7;
                    // literal, line 176
                    if (!(Eq_S_B(1, "u")))
                    {
                        goto lab2;
                    }
                } while (false);
                lab3:
                // delete, line 176
                SliceDel();
            } while (false);
            lab2:
            m_cursor = m_limit - v_6;
            // do, line 177
            v_8 = m_limit - m_cursor;

            do
            {
                // (, line 177
                // [, line 177
                m_ket = m_cursor;
                // literal, line 177
                if (!(Eq_S_B(1, "o")))
                {
                    goto lab5;
                }
                // ], line 177
                m_bra = m_cursor;
                // literal, line 177
                if (!(Eq_S_B(1, "j")))
                {
                    goto lab5;
                }
                // delete, line 177
                SliceDel();
            } while (false);
            lab5:
            m_cursor = m_limit - v_8;
            m_limit_backward = v_2;
            // goto, line 179

            while (true)
            {
                v_9 = m_limit - m_cursor;

                do
                {
                    if (!(OutGroupingB(g_V1, 97, 246)))
                    {
                        goto lab7;
                    }
                    m_cursor = m_limit - v_9;
                    goto golab6;
                } while (false);
                lab7:
                m_cursor = m_limit - v_9;
                if (m_cursor <= m_limit_backward)
                {
                    return false;
                }
                m_cursor--;
            }
            golab6:
            // [, line 179
            m_ket = m_cursor;
            // next, line 179
            if (m_cursor <= m_limit_backward)
            {
                return false;
            }
            m_cursor--;
            // ], line 179
            m_bra = m_cursor;
            // -> x, line 179
            S_x = SliceTo(S_x);
            // name x, line 179
            if (!(Eq_V_B(S_x.ToString())))
            {
                return false;
            }
            // delete, line 179
            SliceDel();
            return true;
        }


        public override bool Stem()
        {
            int v_1;
            int v_2;
            int v_3;
            int v_4;
            int v_5;
            int v_6;
            int v_7;
            int v_8;
            int v_9;
            // (, line 183
            // do, line 185
            v_1 = m_cursor;

            do
            {
                // call mark_regions, line 185
                if (!r_mark_regions())
                {
                    goto lab0;
                }
            } while (false);
            lab0:
            m_cursor = v_1;
            // unset ending_removed, line 186
            B_ending_removed = false;
            // backwards, line 187
            m_limit_backward = m_cursor; m_cursor = m_limit;
            // (, line 187
            // do, line 188
            v_2 = m_limit - m_cursor;

            do
            {
                // call particle_etc, line 188
                if (!r_particle_etc())
                {
                    goto lab1;
                }
            } while (false);
            lab1:
            m_cursor = m_limit - v_2;
            // do, line 189
            v_3 = m_limit - m_cursor;

            do
            {
                // call possessive, line 189
                if (!r_possessive())
                {
                    goto lab2;
                }
            } while (false);
            lab2:
            m_cursor = m_limit - v_3;
            // do, line 190
            v_4 = m_limit - m_cursor;

            do
            {
                // call case_ending, line 190
                if (!r_case_ending())
                {
                    goto lab3;
                }
            } while (false);
            lab3:
            m_cursor = m_limit - v_4;
            // do, line 191
            v_5 = m_limit - m_cursor;

            do
            {
                // call other_endings, line 191
                if (!r_other_endings())
                {
                    goto lab4;
                }
            } while (false);
            lab4:
            m_cursor = m_limit - v_5;
            // or, line 192

            do
            {
                v_6 = m_limit - m_cursor;

                do
                {
                    // (, line 192
                    // Boolean test ending_removed, line 192
                    if (!(B_ending_removed))
                    {
                        goto lab6;
                    }
                    // do, line 192
                    v_7 = m_limit - m_cursor;

                    do
                    {
                        // call i_plural, line 192
                        if (!r_i_plural())
                        {
                            goto lab7;
                        }
                    } while (false);
                    lab7:
                    m_cursor = m_limit - v_7;
                    goto lab5;
                } while (false);
                lab6:
                m_cursor = m_limit - v_6;
                // do, line 192
                v_8 = m_limit - m_cursor;

                do
                {
                    // call t_plural, line 192
                    if (!r_t_plural())
                    {
                        goto lab8;
                    }
                } while (false);
                lab8:
                m_cursor = m_limit - v_8;
            } while (false);
            lab5:
            // do, line 193
            v_9 = m_limit - m_cursor;

            do
            {
                // call tidy, line 193
                if (!r_tidy())
                {
                    goto lab9;
                }
            } while (false);
            lab9:
            m_cursor = m_limit - v_9;
            m_cursor = m_limit_backward; return true;
        }

        public override bool Equals(object o)
        {
            return o is FinnishStemmer;
        }

        public override int GetHashCode()
        {
            return this.GetType().FullName.GetHashCode();
        }
    }
}