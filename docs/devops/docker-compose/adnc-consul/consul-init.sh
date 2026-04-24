#!/bin/bash
set -e  # 如果有错误，立即退出脚本
echo "🚀 正在初始化 Consul K/V..."
# 等待2秒
sleep 2
# 检查 KV 文件是否存在
if [ -f "/consul/kv.json" ]; then
  # consul kv import @/consul/kv.json
  # consul kv export > /consul/kv.json
  consul kv import -http-addr=http://172.25.0.1:8500 @/consul/kv.json
  echo "✅ Consul K/V 数据导入完成"
else
  echo "⚠️ 未找到 /consul/kv.json，跳过 K/V 导入"
fi
echo "🔧 正在启动 Consul 客户端代理..."
# 启动 Consul agent
consul agent \
  -node=consul-client-1 \
  -client=0.0.0.0 \
  -bind=172.25.0.4 \
  -datacenter=adnc_dc \
  -config-dir=/consul/config \
  -data-dir=/consul/data \
  -retry-join=172.25.0.1
echo "🎉 Consul 客户端代理已成功启动！"
