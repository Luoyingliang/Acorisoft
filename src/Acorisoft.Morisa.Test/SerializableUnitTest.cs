using System;
using System.Collections.Generic;
using System.Linq;
using Acorisoft.Morisa.Core;
using LiteDB;
using NUnit.Framework;

namespace Acorisoft.Morisa.Test
{
    [TestFixture]
    public class SerializableUnitTest
    {
        [Test]
        public void SerializeAndDeserialize_PermissionPersistent()
        {
            //
            // string 2 type
            //

            var typeStr = typeof(string).AssemblyQualifiedName;
            var type = Type.GetType(typeStr);
            Assert.AreEqual(type, typeof(string));
            
            //
            // 对象测试
            var pl = new MorisaDocumentSystem.PermissionPersistent
            {
                Owner = typeof(string),
                Target = typeof(string),
                Permission = ResourcePermission.V1_FullControl
            };

            var bson = MorisaDocumentSystem.PermissionManager.Serialize(pl);
            var back = MorisaDocumentSystem.PermissionManager.DeserializePermissionPersistent(bson);

            Assert.AreEqual(pl.Owner, back.Owner);
            Assert.AreEqual(pl.Permission, back.Permission);
            Assert.AreEqual(pl.Target, back.Target);
            
            //
            // 列表测试
            var list = Enumerable.Range(0, 10).Select(i => pl).ToList();
            var bson1 = MorisaDocumentSystem.PermissionManager.Serialize(list);
            var back1 = MorisaDocumentSystem.PermissionManager.DeserializeAll(bson1);

            Assert.AreEqual(list.Count, back1.Count);
            foreach (var item in back1)
            {
                Assert.AreEqual(pl.Owner, item.Owner);
                Assert.AreEqual(pl.Permission, item.Permission);
                Assert.AreEqual(pl.Target, item.Target);
            }
        }
    }
}