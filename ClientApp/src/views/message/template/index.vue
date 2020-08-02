<template>
    <div class="app-container">
        <div class="block">
            <el-row>
                <el-col :span="24">
                    <el-button type="success" size="mini" icon="el-icon-plus" @click.native="add">{{ $t('button.add') }}</el-button>
                    <el-button type="primary" size="mini" icon="el-icon-edit" @click.native="edit">{{ $t('button.edit') }}</el-button>
                    <el-button type="danger" size="mini" icon="el-icon-delete" @click.native="remove">{{ $t('button.delete') }}</el-button>
                </el-col>
            </el-row>
        </div>


        <el-table :data="list" v-loading="listLoading" element-loading-text="Loading" border fit highlight-current-row
                  @current-change="handleCurrentChange">
            <el-table-column label="编号">
                <template slot-scope="scope">
                    {{scope.row.code}}
                </template>
            </el-table-column>
            <el-table-column label="标题">
                <template slot-scope="scope">
                    {{scope.row.title}}
                </template>
            </el-table-column>
            <el-table-column label="内容">
                <template slot-scope="scope">
                    {{scope.row.content}}
                </template>
            </el-table-column>
            <el-table-column label="发送条件">
                <template slot-scope="scope">
                    {{scope.row.cond}}
                </template>
            </el-table-column>
            <el-table-column label="发送器">
                <template slot-scope="scope">
                    {{scope.row.messageSender.name}}
                </template>
            </el-table-column>
        </el-table>

        <el-pagination
                background
                layout="total, sizes, prev, pager, next, jumper"
                :page-sizes="[10, 20, 50, 100,500]"
                :page-size="listQuery.limit"
                :total="total"
                @size-change="changeSize"
                @current-change="fetchPage"
                @prev-click="fetchPrev"
                @next-click="fetchNext">
        </el-pagination>

        <el-dialog
                :title="formTitle"
                :visible.sync="formVisible"
                width="70%">
            <el-form ref="form" :model="form" :rules="rules" label-width="120px">
                <el-row>
                    <el-col :span="12">
                        <el-form-item label="编号"  >
                            <el-input v-model="form.code" minlength=1></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="12">
                        <el-form-item label="标题"  >
                            <el-input v-model="form.title" minlength=1></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="12">
                        <el-form-item label="内容"  >
                            <el-input v-model="form.content" minlength=1></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="12">
                        <el-form-item label="发送条件"  >
                            <el-input v-model="form.cond" minlength=1></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="12">
                        <el-form-item label="发送器"  >
                          <el-select  v-model="form.idMessageSender" filterable placeholder="请选择">
                            <el-option
                              v-for="item in sendList"
                              :key="item.id"
                              :label="item.name"
                              :value="item.id"

                            >
                            </el-option>
                          </el-select>

                        </el-form-item>
                    </el-col>
                </el-row>
                <el-form-item>
                    <el-button type="primary" @click="save">{{ $t('button.submit') }}</el-button>
                    <el-button @click.native="formVisible = false">{{ $t('button.cancel') }}</el-button>
                </el-form-item>

            </el-form>
        </el-dialog>
    </div>
</template>

<script src="./t_message_template.js"></script>


<style rel="stylesheet/scss" lang="scss" scoped>
    @import "src/styles/common.scss";
</style>

