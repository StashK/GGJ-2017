using UnityEngine;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using System;

namespace JPL
{
    public static class Utilities
    {
        public static string Encrypt(string toEncrypt)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(Core.Key);
            // 256-AES key
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            // http://msdn.microsoft.com/en-us/library/system.security.cryptography.ciphermode.aspx
            rDel.Padding = PaddingMode.PKCS7;
            // better lang support
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string toDecrypt)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(Core.Key);
            // AES-256 key
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            // http://msdn.microsoft.com/en-us/library/system.security.cryptography.ciphermode.aspx
            rDel.Padding = PaddingMode.PKCS7;
            // better lang support
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static string Md5Sum(string strToEncrypt)
        {
            System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
            byte[] bytes = ue.GetBytes(strToEncrypt);

            // encrypt bytes
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);

            // Convert the encrypted bytes back to a string (base 16)
            string hashString = "";

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }

            return hashString.PadLeft(32, '0');
        }

        public static Vector3 BezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Vector3 point = Vector3.zero;

            float u = 1.0f - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            point = uuu * p0;
            point += 3 * uu * t * p1;
            point += 3 * u * tt * p2;
            point += ttt * p3;

            return point;
        }

        public static float BezierPointHighest(float steps, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float highest = 0f;
            Vector3 highestPos = Vector3.zero;

            for (int i = 0; i < steps; i++)
            {
                if (BezierPoint(1f / steps * i, p0, p1, p2, p3).y > highestPos.y)
                {
                    highestPos = BezierPoint(1f / steps * i, p0, p1, p2, p3);
                    highest = 1f / steps * i;
                }
            }

            return highest;
        }

        public static void DebugBezier(int steps, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            // debug the curve
            for (int i = 0; i < steps; i++)
            {
                Debug.DrawLine(BezierPoint(1f / steps * i, p0, p1, p2, p3), BezierPoint(1f / steps * (i + 1), p0, p1, p2, p3));
            }

            // debug the handles
            Debug.DrawLine(p0, p1, Color.red);
            Debug.DrawLine(p2, p3, Color.red);
        }

        public static Material RaycastHitMaterial (RaycastHit hit)
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            MeshCollider meshCollider = hit.collider as MeshCollider;

            if (renderer != null && renderer.material != null)
            {
                return renderer.material;
            }
            else if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null || meshCollider == null)
            {
                Debug.Log("nope..");
                return null;
            }

            int materialIdx = -1;

            Mesh mesh = meshCollider.sharedMesh;
            int triangleIdx = hit.triangleIndex;
            int lookupIdx1 = mesh.triangles[triangleIdx * 3];
            int lookupIdx2 = mesh.triangles[triangleIdx * 3 + 1];
            int lookupIdx3 = mesh.triangles[triangleIdx * 3 + 2];

            int subMeshesNr = mesh.subMeshCount;
            for (int i = 0; i < subMeshesNr; i++)
            {
                int[] tr = mesh.GetTriangles(i);
                for (int j = 0; j < tr.Length; j += 3)
                {
                    if (tr[j] == lookupIdx1 && tr[j + 1] == lookupIdx2 && tr[j + 2] == lookupIdx3)
                    {
                        materialIdx = i;
                        break;
                    }
                }
                if (materialIdx != -1) break;
            }

            if (materialIdx != -1)
            {
                Debug.Log("-------------------- I'm using " + renderer.materials[materialIdx].name + " material(s)");
                return renderer.materials[materialIdx];
            }

            return null;
        }
    }
}