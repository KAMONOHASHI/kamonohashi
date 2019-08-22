using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Types;

namespace Nssol.Platypus.Migrations
{
    public partial class v100 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gits",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ServiceType = table.Column<int>(nullable: false),
                    ApiUrl = table.Column<string>(nullable: false),
                    Token = table.Column<string>(nullable: true),
                    RepositoryUrl = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nodes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Memo = table.Column<string>(nullable: true),
                    AccessLevel = table.Column<int>(nullable: false),
                    Partition = table.Column<string>(nullable: true),
                    TensorBoardEnabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Registries",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Host = table.Column<string>(nullable: false),
                    PortNo = table.Column<int>(nullable: false),
                    ServiceType = table.Column<int>(nullable: false),
                    ProjectName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    ApiUrl = table.Column<string>(nullable: true),
                    RegistryUrl = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    EnsureSingleRow = table.Column<int>(nullable: false),
                    ApiSecurityTokenPass = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Storages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ServerAddress = table.Column<string>(nullable: false),
                    AccessKey = table.Column<string>(nullable: false),
                    SecretKey = table.Column<string>(nullable: false),
                    NfsServer = table.Column<string>(nullable: false),
                    NfsRoot = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    DisplayName = table.Column<string>(nullable: false),
                    StorageId = table.Column<long>(nullable: true),
                    StorageBucket = table.Column<string>(nullable: true),
                    DefaultGitId = table.Column<long>(nullable: false),
                    DefaultRegistryId = table.Column<long>(nullable: false),
                    LimitCpu = table.Column<int>(nullable: true),
                    LimitMemory = table.Column<int>(nullable: true),
                    LimitGpu = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tenants_Gits_DefaultGitId",
                        column: x => x.DefaultGitId,
                        principalTable: "Gits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tenants_Registries_DefaultRegistryId",
                        column: x => x.DefaultRegistryId,
                        principalTable: "Registries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tenants_Storages_StorageId",
                        column: x => x.StorageId,
                        principalTable: "Storages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Data",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    DisplayId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Memo = table.Column<string>(nullable: true),
                    ParentDataId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Data_Data_ParentDataId",
                        column: x => x.ParentDataId,
                        principalTable: "Data",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Data_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataFiles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    FileName = table.Column<string>(nullable: false),
                    StoredPath = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataFiles_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataSets",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    DisplayId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Memo = table.Column<string>(nullable: true),
                    IsLocked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataSets_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataTypes_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NodeTenantMaps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    NodeId = table.Column<long>(nullable: false),
                    TenantId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeTenantMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NodeTenantMaps_Nodes_NodeId",
                        column: x => x.NodeId,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NodeTenantMaps_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Preprocesses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    DisplayId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    EntryPoint = table.Column<string>(nullable: true),
                    RepositoryGitId = table.Column<long>(nullable: true),
                    RepositoryName = table.Column<string>(nullable: true),
                    RepositoryOwner = table.Column<string>(nullable: true),
                    RepositoryBranch = table.Column<string>(nullable: true),
                    RepositoryCommitId = table.Column<string>(nullable: true),
                    ContainerRegistryId = table.Column<long>(nullable: true),
                    ContainerImage = table.Column<string>(nullable: true),
                    ContainerTag = table.Column<string>(nullable: true),
                    Memo = table.Column<string>(nullable: true),
                    Cpu = table.Column<int>(nullable: false),
                    Memory = table.Column<int>(nullable: false),
                    Gpu = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preprocesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Preprocesses_Registries_ContainerRegistryId",
                        column: x => x.ContainerRegistryId,
                        principalTable: "Registries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Preprocesses_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    DisplayName = table.Column<string>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false),
                    TenantId = table.Column<long>(nullable: true),
                    IsSystemRole = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TenantGitMaps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    GitId = table.Column<long>(nullable: false),
                    IsEditable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantGitMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantGitMaps_Gits_GitId",
                        column: x => x.GitId,
                        principalTable: "Gits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TenantGitMaps_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TenantRegistryMaps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    RegistryId = table.Column<long>(nullable: false),
                    IsEditable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantRegistryMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantRegistryMaps_Registries_RegistryId",
                        column: x => x.RegistryId,
                        principalTable: "Registries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TenantRegistryMaps_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Alias = table.Column<string>(nullable: true),
                    DefaultTenantId = table.Column<long>(nullable: false),
                    ServiceType = table.Column<int>(nullable: false),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Tenants_DefaultTenantId",
                        column: x => x.DefaultTenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DataProperties",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    DataId = table.Column<long>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    DataString = table.Column<string>(nullable: true),
                    DataFileId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataProperties_DataFiles_DataFileId",
                        column: x => x.DataFileId,
                        principalTable: "DataFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataProperties_Data_DataId",
                        column: x => x.DataId,
                        principalTable: "Data",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataProperties_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingHistories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    DisplayId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    DataSetId = table.Column<long>(nullable: false),
                    ModelGitId = table.Column<long>(nullable: false),
                    ModelRepository = table.Column<string>(nullable: false),
                    ModelRepositoryOwner = table.Column<string>(nullable: false),
                    ModelBranch = table.Column<string>(nullable: true),
                    ModelCommitId = table.Column<string>(nullable: false),
                    EntryPoint = table.Column<string>(nullable: false),
                    ContainerRegistryId = table.Column<long>(nullable: false),
                    ContainerImage = table.Column<string>(nullable: false),
                    ContainerTag = table.Column<string>(nullable: false),
                    Options = table.Column<string>(nullable: true),
                    ParentId = table.Column<long>(nullable: true),
                    Cpu = table.Column<int>(nullable: false),
                    Memory = table.Column<int>(nullable: false),
                    Gpu = table.Column<int>(nullable: false),
                    Partition = table.Column<string>(nullable: true),
                    Configuration = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: false),
                    Node = table.Column<string>(nullable: true),
                    StartedAt = table.Column<DateTime>(nullable: true),
                    CompletedAt = table.Column<DateTime>(nullable: true),
                    LogSummary = table.Column<string>(nullable: true),
                    Memo = table.Column<string>(nullable: true),
                    Favorite = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingHistories_Registries_ContainerRegistryId",
                        column: x => x.ContainerRegistryId,
                        principalTable: "Registries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingHistories_DataSets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "DataSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingHistories_TrainingHistories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "TrainingHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainingHistories_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataSetEntries",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    DataSetId = table.Column<long>(nullable: false),
                    DataId = table.Column<long>(nullable: false),
                    DataTypeId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSetEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataSetEntries_Data_DataId",
                        column: x => x.DataId,
                        principalTable: "Data",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataSetEntries_DataSets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "DataSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataSetEntries_DataTypes_DataTypeId",
                        column: x => x.DataTypeId,
                        principalTable: "DataTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataSetEntries_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreprocessHistories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    InputDataId = table.Column<long>(nullable: false),
                    PreprocessId = table.Column<long>(nullable: true),
                    StartedAt = table.Column<DateTime>(nullable: true),
                    CompletedAt = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Memo = table.Column<string>(nullable: true),
                    ContainerIdentifier = table.Column<string>(nullable: true),
                    Options = table.Column<string>(nullable: true),
                    Cpu = table.Column<int>(nullable: true),
                    Memory = table.Column<int>(nullable: true),
                    Gpu = table.Column<int>(nullable: true),
                    Partition = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreprocessHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreprocessHistories_Data_InputDataId",
                        column: x => x.InputDataId,
                        principalTable: "Data",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreprocessHistories_Preprocesses_PreprocessId",
                        column: x => x.PreprocessId,
                        principalTable: "Preprocesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreprocessHistories_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuRoleMaps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    MenuCode = table.Column<string>(nullable: false),
                    RoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuRoleMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuRoleMaps_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataTagMaps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    DataId = table.Column<long>(nullable: false),
                    TagId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataTagMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataTagMaps_Data_DataId",
                        column: x => x.DataId,
                        principalTable: "Data",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataTagMaps_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataTagMaps_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTenantGitMaps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    TenantGitMapId = table.Column<long>(nullable: false),
                    GitToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTenantGitMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTenantGitMaps_TenantGitMaps_TenantGitMapId",
                        column: x => x.TenantGitMapId,
                        principalTable: "TenantGitMaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTenantGitMaps_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTenantMaps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    ClusterToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTenantMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTenantMaps_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTenantMaps_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTenantRegistryMaps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    TenantRegistryMapId = table.Column<long>(nullable: false),
                    RegistryUserName = table.Column<string>(nullable: true),
                    RegistryPassword = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTenantRegistryMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTenantRegistryMaps_TenantRegistryMaps_TenantRegistryMap~",
                        column: x => x.TenantRegistryMapId,
                        principalTable: "TenantRegistryMaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTenantRegistryMaps_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InferenceHistories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    DisplayId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    DataSetId = table.Column<long>(nullable: false),
                    ModelGitId = table.Column<long>(nullable: false),
                    ModelRepository = table.Column<string>(nullable: false),
                    ModelRepositoryOwner = table.Column<string>(nullable: false),
                    ModelBranch = table.Column<string>(nullable: true),
                    ModelCommitId = table.Column<string>(nullable: false),
                    EntryPoint = table.Column<string>(nullable: false),
                    ContainerRegistryId = table.Column<long>(nullable: false),
                    ContainerImage = table.Column<string>(nullable: false),
                    ContainerTag = table.Column<string>(nullable: false),
                    Options = table.Column<string>(nullable: true),
                    ParentId = table.Column<long>(nullable: true),
                    Cpu = table.Column<int>(nullable: false),
                    Memory = table.Column<int>(nullable: false),
                    Gpu = table.Column<int>(nullable: false),
                    Partition = table.Column<string>(nullable: true),
                    Configuration = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: false),
                    Node = table.Column<string>(nullable: true),
                    StartedAt = table.Column<DateTime>(nullable: true),
                    CompletedAt = table.Column<DateTime>(nullable: true),
                    LogSummary = table.Column<string>(nullable: true),
                    Memo = table.Column<string>(nullable: true),
                    Favorite = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InferenceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InferenceHistories_Registries_ContainerRegistryId",
                        column: x => x.ContainerRegistryId,
                        principalTable: "Registries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InferenceHistories_DataSets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "DataSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InferenceHistories_TrainingHistories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "TrainingHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InferenceHistories_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TensorBoardContainers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Host = table.Column<string>(nullable: true),
                    PortNo = table.Column<int>(nullable: true),
                    TrainingHistoryId = table.Column<long>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    StartedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TensorBoardContainers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TensorBoardContainers_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TensorBoardContainers_TrainingHistories_TrainingHistoryId",
                        column: x => x.TrainingHistoryId,
                        principalTable: "TrainingHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingHistoryAttachedFiles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    TrainingHistoryId = table.Column<long>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: false),
                    StoredPath = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingHistoryAttachedFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingHistoryAttachedFiles_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingHistoryAttachedFiles_TrainingHistories_TrainingHist~",
                        column: x => x.TrainingHistoryId,
                        principalTable: "TrainingHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingHistoryTagMaps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    TrainingHistoryId = table.Column<long>(nullable: false),
                    TagId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingHistoryTagMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingHistoryTagMaps_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingHistoryTagMaps_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingHistoryTagMaps_TrainingHistories_TrainingHistoryId",
                        column: x => x.TrainingHistoryId,
                        principalTable: "TrainingHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreprocessHistoryOutputs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    PreprocessHistoryId = table.Column<long>(nullable: false),
                    OutputDataId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreprocessHistoryOutputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreprocessHistoryOutputs_Data_OutputDataId",
                        column: x => x.OutputDataId,
                        principalTable: "Data",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreprocessHistoryOutputs_PreprocessHistories_PreprocessHist~",
                        column: x => x.PreprocessHistoryId,
                        principalTable: "PreprocessHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreprocessHistoryOutputs_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleMaps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false),
                    TenantMapId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoleMaps_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoleMaps_UserTenantMaps_TenantMapId",
                        column: x => x.TenantMapId,
                        principalTable: "UserTenantMaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoleMaps_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InferenceHistoryAttachedFiles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    InferenceHistoryId = table.Column<long>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: false),
                    StoredPath = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InferenceHistoryAttachedFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InferenceHistoryAttachedFiles_InferenceHistories_InferenceH~",
                        column: x => x.InferenceHistoryId,
                        principalTable: "InferenceHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InferenceHistoryAttachedFiles_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Data_ParentDataId",
                table: "Data",
                column: "ParentDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Data_TenantId",
                table: "Data",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_DataFiles_TenantId",
                table: "DataFiles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_DataProperties_DataFileId",
                table: "DataProperties",
                column: "DataFileId");

            migrationBuilder.CreateIndex(
                name: "IX_DataProperties_DataId",
                table: "DataProperties",
                column: "DataId");

            migrationBuilder.CreateIndex(
                name: "IX_DataProperties_TenantId",
                table: "DataProperties",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSetEntries_DataId",
                table: "DataSetEntries",
                column: "DataId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSetEntries_DataSetId",
                table: "DataSetEntries",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSetEntries_DataTypeId",
                table: "DataSetEntries",
                column: "DataTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSetEntries_TenantId",
                table: "DataSetEntries",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSets_TenantId",
                table: "DataSets",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_DataTagMaps_DataId",
                table: "DataTagMaps",
                column: "DataId");

            migrationBuilder.CreateIndex(
                name: "IX_DataTagMaps_TagId",
                table: "DataTagMaps",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_DataTagMaps_TenantId",
                table: "DataTagMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_DataTypes_TenantId",
                table: "DataTypes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceHistories_ContainerRegistryId",
                table: "InferenceHistories",
                column: "ContainerRegistryId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceHistories_DataSetId",
                table: "InferenceHistories",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceHistories_ParentId",
                table: "InferenceHistories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceHistories_TenantId",
                table: "InferenceHistories",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceHistoryAttachedFiles_InferenceHistoryId",
                table: "InferenceHistoryAttachedFiles",
                column: "InferenceHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceHistoryAttachedFiles_TenantId",
                table: "InferenceHistoryAttachedFiles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuRoleMaps_RoleId",
                table: "MenuRoleMaps",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuRoleMaps_MenuCode_RoleId",
                table: "MenuRoleMaps",
                columns: new[] { "MenuCode", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NodeTenantMaps_TenantId",
                table: "NodeTenantMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_NodeTenantMaps_NodeId_TenantId",
                table: "NodeTenantMaps",
                columns: new[] { "NodeId", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Preprocesses_ContainerRegistryId",
                table: "Preprocesses",
                column: "ContainerRegistryId");

            migrationBuilder.CreateIndex(
                name: "IX_Preprocesses_TenantId",
                table: "Preprocesses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PreprocessHistories_InputDataId",
                table: "PreprocessHistories",
                column: "InputDataId");

            migrationBuilder.CreateIndex(
                name: "IX_PreprocessHistories_PreprocessId",
                table: "PreprocessHistories",
                column: "PreprocessId");

            migrationBuilder.CreateIndex(
                name: "IX_PreprocessHistories_TenantId",
                table: "PreprocessHistories",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PreprocessHistoryOutputs_OutputDataId",
                table: "PreprocessHistoryOutputs",
                column: "OutputDataId");

            migrationBuilder.CreateIndex(
                name: "IX_PreprocessHistoryOutputs_PreprocessHistoryId",
                table: "PreprocessHistoryOutputs",
                column: "PreprocessHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PreprocessHistoryOutputs_TenantId",
                table: "PreprocessHistoryOutputs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Registries_Name",
                table: "Registries",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_TenantId",
                table: "Roles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_ApiSecurityTokenPass",
                table: "Settings",
                column: "ApiSecurityTokenPass",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_TenantId",
                table: "Tags",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantGitMaps_GitId",
                table: "TenantGitMaps",
                column: "GitId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantGitMaps_TenantId_GitId",
                table: "TenantGitMaps",
                columns: new[] { "TenantId", "GitId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenantRegistryMaps_RegistryId",
                table: "TenantRegistryMaps",
                column: "RegistryId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantRegistryMaps_TenantId_RegistryId",
                table: "TenantRegistryMaps",
                columns: new[] { "TenantId", "RegistryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_DefaultGitId",
                table: "Tenants",
                column: "DefaultGitId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_DefaultRegistryId",
                table: "Tenants",
                column: "DefaultRegistryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Name",
                table: "Tenants",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_StorageId",
                table: "Tenants",
                column: "StorageId");

            migrationBuilder.CreateIndex(
                name: "IX_TensorBoardContainers_TenantId",
                table: "TensorBoardContainers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TensorBoardContainers_TrainingHistoryId",
                table: "TensorBoardContainers",
                column: "TrainingHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingHistories_ContainerRegistryId",
                table: "TrainingHistories",
                column: "ContainerRegistryId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingHistories_DataSetId",
                table: "TrainingHistories",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingHistories_ParentId",
                table: "TrainingHistories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingHistories_TenantId",
                table: "TrainingHistories",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingHistoryAttachedFiles_TenantId",
                table: "TrainingHistoryAttachedFiles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingHistoryAttachedFiles_TrainingHistoryId",
                table: "TrainingHistoryAttachedFiles",
                column: "TrainingHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingHistoryTagMaps_TagId",
                table: "TrainingHistoryTagMaps",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingHistoryTagMaps_TenantId",
                table: "TrainingHistoryTagMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingHistoryTagMaps_TrainingHistoryId",
                table: "TrainingHistoryTagMaps",
                column: "TrainingHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleMaps_RoleId",
                table: "UserRoleMaps",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleMaps_TenantMapId",
                table: "UserRoleMaps",
                column: "TenantMapId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleMaps_UserId_RoleId_TenantMapId",
                table: "UserRoleMaps",
                columns: new[] { "UserId", "RoleId", "TenantMapId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_DefaultTenantId",
                table: "Users",
                column: "DefaultTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTenantGitMaps_TenantGitMapId",
                table: "UserTenantGitMaps",
                column: "TenantGitMapId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTenantGitMaps_UserId_TenantGitMapId",
                table: "UserTenantGitMaps",
                columns: new[] { "UserId", "TenantGitMapId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTenantMaps_UserId",
                table: "UserTenantMaps",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTenantMaps_TenantId_UserId",
                table: "UserTenantMaps",
                columns: new[] { "TenantId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTenantRegistryMaps_TenantRegistryMapId",
                table: "UserTenantRegistryMaps",
                column: "TenantRegistryMapId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTenantRegistryMaps_UserId_TenantRegistryMapId",
                table: "UserTenantRegistryMaps",
                columns: new[] { "UserId", "TenantRegistryMapId" },
                unique: true);

            // 共通変数
            string adminUser = ApplicationConst.DefaultFirstAdminUserName;
            DateTime now = DateTime.Now;

            // ロール作成
            migrationBuilder.Sql("INSERT INTO \"Roles\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"Name\", \"DisplayName\", \"SortOrder\", \"TenantId\", \"IsSystemRole\") VALUES( nextval('\"Roles_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', 'users', 'User', 10, null, false);");
            migrationBuilder.Sql("INSERT INTO \"Roles\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"Name\", \"DisplayName\", \"SortOrder\", \"TenantId\", \"IsSystemRole\") VALUES( nextval('\"Roles_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', 'researchers', 'Researcher', 20, null, false);");
            migrationBuilder.Sql("INSERT INTO \"Roles\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"Name\", \"DisplayName\", \"SortOrder\", \"TenantId\", \"IsSystemRole\") VALUES( nextval('\"Roles_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', 'managers', 'Manager', 30, null, false);");
            migrationBuilder.Sql("INSERT INTO \"Roles\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"Name\", \"DisplayName\", \"SortOrder\", \"TenantId\", \"IsSystemRole\") VALUES( nextval('\"Roles_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', 'admins', 'Admin', 100, null, false);");

            // メニュー作成
            migrationBuilder.Sql("INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', '" + Logic.MenuLogic.DataMenu.Code.ToString() + "', \"Id\" as RoleId FROM \"Roles\" WHERE \"Name\" = 'users' AND \"DisplayName\" = 'User';");
            migrationBuilder.Sql("INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', '" + Logic.MenuLogic.DataSetMenu.Code.ToString() + "', \"Id\" as RoleId FROM \"Roles\" WHERE \"Name\" = 'users' AND \"DisplayName\" = 'User';");
            
            migrationBuilder.Sql("INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', '" + Logic.MenuLogic.DataMenu.Code.ToString() + "', \"Id\" as RoleId FROM \"Roles\" WHERE \"Name\" = 'researchers' AND \"DisplayName\" = 'Researcher';");
            migrationBuilder.Sql("INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', '" + Logic.MenuLogic.DataSetMenu.Code.ToString() + "', \"Id\" as RoleId FROM \"Roles\" WHERE \"Name\" = 'researchers' AND \"DisplayName\" = 'Researcher';");
            migrationBuilder.Sql("INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', '" + Logic.MenuLogic.PreprocessMenu.Code.ToString() + "', \"Id\" as RoleId FROM \"Roles\" WHERE \"Name\" = 'researchers' AND \"DisplayName\" = 'Researcher';");
            migrationBuilder.Sql("INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', '" + Logic.MenuLogic.TrainingMenu.Code.ToString() + "', \"Id\" as RoleId FROM \"Roles\" WHERE \"Name\" = 'researchers' AND \"DisplayName\" = 'Researcher';");
            migrationBuilder.Sql("INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', '" + Logic.MenuLogic.InferenceMenu.Code.ToString() + "', \"Id\" as RoleId FROM \"Roles\" WHERE \"Name\" = 'researchers' AND \"DisplayName\" = 'Researcher';");

            migrationBuilder.Sql("INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', '" + Logic.MenuLogic.TenantSettingMenu.Code.ToString() + "', \"Id\" as RoleId FROM \"Roles\" WHERE \"Name\" = 'managers' AND \"DisplayName\" = 'Manager';");
            migrationBuilder.Sql("INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', '" + Logic.MenuLogic.TenantRoleMenu.Code.ToString() + "', \"Id\" as RoleId FROM \"Roles\" WHERE \"Name\" = 'managers' AND \"DisplayName\" = 'Manager';");
            migrationBuilder.Sql("INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', '" + Logic.MenuLogic.TenantUserMenu.Code.ToString() + "', \"Id\" as RoleId FROM \"Roles\" WHERE \"Name\" = 'managers' AND \"DisplayName\" = 'Manager';");
            migrationBuilder.Sql("INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', '" + Logic.MenuLogic.TenantMenuAccessMenu.Code.ToString() + "', \"Id\" as RoleId FROM \"Roles\" WHERE \"Name\" = 'managers' AND \"DisplayName\" = 'Manager';");
            migrationBuilder.Sql("INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', '" + Logic.MenuLogic.TenantResourceMenu.Code.ToString() + "', \"Id\" as RoleId FROM \"Roles\" WHERE \"Name\" = 'managers' AND \"DisplayName\" = 'Manager';");

            migrationBuilder.Sql("INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', '" + Logic.MenuLogic.TenantMenu.Code.ToString() + "', \"Id\" as RoleId FROM \"Roles\" WHERE \"Name\" = 'admins' AND \"DisplayName\" = 'Admin';");
            migrationBuilder.Sql("INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', '" + Logic.MenuLogic.GitMenu.Code.ToString() + "', \"Id\" as RoleId FROM \"Roles\" WHERE \"Name\" = 'admins' AND \"DisplayName\" = 'Admin';");
            migrationBuilder.Sql("INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', '" + Logic.MenuLogic.RegistryMenu.Code.ToString() + "', \"Id\" as RoleId FROM \"Roles\" WHERE \"Name\" = 'admins' AND \"DisplayName\" = 'Admin';");
            migrationBuilder.Sql("INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', '" + Logic.MenuLogic.StorageMenu.Code.ToString() + "', \"Id\" as RoleId FROM \"Roles\" WHERE \"Name\" = 'admins' AND \"DisplayName\" = 'Admin';");
            migrationBuilder.Sql("INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', '" + Logic.MenuLogic.RoleMenu.Code.ToString() + "', \"Id\" as RoleId FROM \"Roles\" WHERE \"Name\" = 'admins' AND \"DisplayName\" = 'Admin';");
            migrationBuilder.Sql("INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', '" + Logic.MenuLogic.QuotaMenu.Code.ToString() + "', \"Id\" as RoleId FROM \"Roles\" WHERE \"Name\" = 'admins' AND \"DisplayName\" = 'Admin';");
            migrationBuilder.Sql("INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', '" + Logic.MenuLogic.UserMenu.Code.ToString() + "', \"Id\" as RoleId FROM \"Roles\" WHERE \"Name\" = 'admins' AND \"DisplayName\" = 'Admin';");
            migrationBuilder.Sql("INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', '" + Logic.MenuLogic.NodeMenu.Code.ToString() + "', \"Id\" as RoleId FROM \"Roles\" WHERE \"Name\" = 'admins' AND \"DisplayName\" = 'Admin';");
            migrationBuilder.Sql("INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', '" + Logic.MenuLogic.ResourceMenu.Code.ToString() + "', \"Id\" as RoleId FROM \"Roles\" WHERE \"Name\" = 'admins' AND \"DisplayName\" = 'Admin';");
            migrationBuilder.Sql("INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', '" + Logic.MenuLogic.MenuAccessMenu.Code.ToString() + "', \"Id\" as RoleId FROM \"Roles\" WHERE \"Name\" = 'admins' AND \"DisplayName\" = 'Admin';");
            
            // 初期Git作成
            migrationBuilder.Sql("INSERT INTO \"Gits\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"Name\", \"ServiceType\", \"ApiUrl\", \"Token\", \"RepositoryUrl\") VALUES( nextval('\"Gits_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', 'GitHub', " + (int)GitServiceType.GitHub + ", 'https://api.github.com', null, 'https://github.com');");
            // 初期Registry作成
            migrationBuilder.Sql("INSERT INTO \"Registries\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"Name\", \"Host\", \"PortNo\", \"ServiceType\", \"ProjectName\", \"Password\", \"ApiUrl\", \"RegistryUrl\") VALUES( nextval('\"Registries_Id_seq\"'), '" + adminUser + "', '" + now + "', '" + adminUser + "', '" + now + "', 'official-docker-hub', 'registry.hub.docker.com', 80, " + (int)RegistryServiceType.DockerHub + ", null, null, 'https://registry.hub.docker.com/', 'https://registry.hub.docker.com/');");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataProperties");

            migrationBuilder.DropTable(
                name: "DataSetEntries");

            migrationBuilder.DropTable(
                name: "DataTagMaps");

            migrationBuilder.DropTable(
                name: "InferenceHistoryAttachedFiles");

            migrationBuilder.DropTable(
                name: "MenuRoleMaps");

            migrationBuilder.DropTable(
                name: "NodeTenantMaps");

            migrationBuilder.DropTable(
                name: "PreprocessHistoryOutputs");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "TensorBoardContainers");

            migrationBuilder.DropTable(
                name: "TrainingHistoryAttachedFiles");

            migrationBuilder.DropTable(
                name: "TrainingHistoryTagMaps");

            migrationBuilder.DropTable(
                name: "UserRoleMaps");

            migrationBuilder.DropTable(
                name: "UserTenantGitMaps");

            migrationBuilder.DropTable(
                name: "UserTenantRegistryMaps");

            migrationBuilder.DropTable(
                name: "DataFiles");

            migrationBuilder.DropTable(
                name: "DataTypes");

            migrationBuilder.DropTable(
                name: "InferenceHistories");

            migrationBuilder.DropTable(
                name: "Nodes");

            migrationBuilder.DropTable(
                name: "PreprocessHistories");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UserTenantMaps");

            migrationBuilder.DropTable(
                name: "TenantGitMaps");

            migrationBuilder.DropTable(
                name: "TenantRegistryMaps");

            migrationBuilder.DropTable(
                name: "TrainingHistories");

            migrationBuilder.DropTable(
                name: "Data");

            migrationBuilder.DropTable(
                name: "Preprocesses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "DataSets");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "Gits");

            migrationBuilder.DropTable(
                name: "Registries");

            migrationBuilder.DropTable(
                name: "Storages");
        }
    }
}
