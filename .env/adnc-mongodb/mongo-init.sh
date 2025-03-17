mongosh -- "$MONGO_INITDB_DATABASE" <<EOF
db = db.getSiblingDB('admin')
db.auth('$MONGO_INITDB_ROOT_USERNAME', '$MONGO_INITDB_ROOT_PASSWORD')
db = db.getSiblingDB('$MONGO_INITDB_DATABASE')
db.createUser({
  user: "$MONGO_USERNAME",
  pwd: "$MONGO_PASSWORD",
  roles: [
  { role: 'readWrite', db: '$MONGO_INITDB_DATABASE' }
  ]
})
db.createCollection("test")
EOF