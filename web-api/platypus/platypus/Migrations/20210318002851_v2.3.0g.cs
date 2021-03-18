using Microsoft.EntityFrameworkCore.Migrations;

namespace Nssol.Platypus.Migrations
{
    public partial class v230g : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiment_AquariumDatasets_DataSetId",
                table: "Experiment");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiment_AquariumDatasetVersions_DataSetVersionId",
                table: "Experiment");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiment_ExperimentPreprocess_ExperimentPreprocessId",
                table: "Experiment");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiment_Templates_TemplateId",
                table: "Experiment");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiment_TemplateVersions_TemplateVersionId",
                table: "Experiment");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiment_Tenants_TenantId",
                table: "Experiment");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiment_TrainingHistories_TrainingHistoryId",
                table: "Experiment");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentPreprocess_AquariumDatasets_DataSetId",
                table: "ExperimentPreprocess");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentPreprocess_AquariumDatasetVersions_DataSetVersion~",
                table: "ExperimentPreprocess");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentPreprocess_Templates_TemplateId",
                table: "ExperimentPreprocess");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentPreprocess_TemplateVersions_TemplateVersionId",
                table: "ExperimentPreprocess");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentPreprocess_Tenants_TenantId",
                table: "ExperimentPreprocess");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentPreprocess_TrainingHistories_TrainingHistoryId",
                table: "ExperimentPreprocess");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExperimentPreprocess",
                table: "ExperimentPreprocess");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Experiment",
                table: "Experiment");

            migrationBuilder.RenameTable(
                name: "ExperimentPreprocess",
                newName: "ExperimentPreprocesses");

            migrationBuilder.RenameTable(
                name: "Experiment",
                newName: "Experiments");

            migrationBuilder.RenameIndex(
                name: "IX_ExperimentPreprocess_TrainingHistoryId",
                table: "ExperimentPreprocesses",
                newName: "IX_ExperimentPreprocesses_TrainingHistoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ExperimentPreprocess_TenantId",
                table: "ExperimentPreprocesses",
                newName: "IX_ExperimentPreprocesses_TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_ExperimentPreprocess_TemplateVersionId",
                table: "ExperimentPreprocesses",
                newName: "IX_ExperimentPreprocesses_TemplateVersionId");

            migrationBuilder.RenameIndex(
                name: "IX_ExperimentPreprocess_TemplateId",
                table: "ExperimentPreprocesses",
                newName: "IX_ExperimentPreprocesses_TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_ExperimentPreprocess_DataSetVersionId",
                table: "ExperimentPreprocesses",
                newName: "IX_ExperimentPreprocesses_DataSetVersionId");

            migrationBuilder.RenameIndex(
                name: "IX_ExperimentPreprocess_DataSetId",
                table: "ExperimentPreprocesses",
                newName: "IX_ExperimentPreprocesses_DataSetId");

            migrationBuilder.RenameIndex(
                name: "IX_Experiment_TrainingHistoryId",
                table: "Experiments",
                newName: "IX_Experiments_TrainingHistoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Experiment_TenantId",
                table: "Experiments",
                newName: "IX_Experiments_TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_Experiment_TemplateVersionId",
                table: "Experiments",
                newName: "IX_Experiments_TemplateVersionId");

            migrationBuilder.RenameIndex(
                name: "IX_Experiment_TemplateId",
                table: "Experiments",
                newName: "IX_Experiments_TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_Experiment_ExperimentPreprocessId",
                table: "Experiments",
                newName: "IX_Experiments_ExperimentPreprocessId");

            migrationBuilder.RenameIndex(
                name: "IX_Experiment_DataSetVersionId",
                table: "Experiments",
                newName: "IX_Experiments_DataSetVersionId");

            migrationBuilder.RenameIndex(
                name: "IX_Experiment_DataSetId",
                table: "Experiments",
                newName: "IX_Experiments_DataSetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExperimentPreprocesses",
                table: "ExperimentPreprocesses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Experiments",
                table: "Experiments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentPreprocesses_AquariumDatasets_DataSetId",
                table: "ExperimentPreprocesses",
                column: "DataSetId",
                principalTable: "AquariumDatasets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentPreprocesses_AquariumDatasetVersions_DataSetVersi~",
                table: "ExperimentPreprocesses",
                column: "DataSetVersionId",
                principalTable: "AquariumDatasetVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentPreprocesses_Templates_TemplateId",
                table: "ExperimentPreprocesses",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentPreprocesses_TemplateVersions_TemplateVersionId",
                table: "ExperimentPreprocesses",
                column: "TemplateVersionId",
                principalTable: "TemplateVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentPreprocesses_Tenants_TenantId",
                table: "ExperimentPreprocesses",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentPreprocesses_TrainingHistories_TrainingHistoryId",
                table: "ExperimentPreprocesses",
                column: "TrainingHistoryId",
                principalTable: "TrainingHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Experiments_AquariumDatasets_DataSetId",
                table: "Experiments",
                column: "DataSetId",
                principalTable: "AquariumDatasets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Experiments_AquariumDatasetVersions_DataSetVersionId",
                table: "Experiments",
                column: "DataSetVersionId",
                principalTable: "AquariumDatasetVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Experiments_ExperimentPreprocesses_ExperimentPreprocessId",
                table: "Experiments",
                column: "ExperimentPreprocessId",
                principalTable: "ExperimentPreprocesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Experiments_Templates_TemplateId",
                table: "Experiments",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Experiments_TemplateVersions_TemplateVersionId",
                table: "Experiments",
                column: "TemplateVersionId",
                principalTable: "TemplateVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Experiments_Tenants_TenantId",
                table: "Experiments",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Experiments_TrainingHistories_TrainingHistoryId",
                table: "Experiments",
                column: "TrainingHistoryId",
                principalTable: "TrainingHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentPreprocesses_AquariumDatasets_DataSetId",
                table: "ExperimentPreprocesses");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentPreprocesses_AquariumDatasetVersions_DataSetVersi~",
                table: "ExperimentPreprocesses");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentPreprocesses_Templates_TemplateId",
                table: "ExperimentPreprocesses");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentPreprocesses_TemplateVersions_TemplateVersionId",
                table: "ExperimentPreprocesses");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentPreprocesses_Tenants_TenantId",
                table: "ExperimentPreprocesses");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentPreprocesses_TrainingHistories_TrainingHistoryId",
                table: "ExperimentPreprocesses");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiments_AquariumDatasets_DataSetId",
                table: "Experiments");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiments_AquariumDatasetVersions_DataSetVersionId",
                table: "Experiments");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiments_ExperimentPreprocesses_ExperimentPreprocessId",
                table: "Experiments");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiments_Templates_TemplateId",
                table: "Experiments");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiments_TemplateVersions_TemplateVersionId",
                table: "Experiments");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiments_Tenants_TenantId",
                table: "Experiments");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiments_TrainingHistories_TrainingHistoryId",
                table: "Experiments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Experiments",
                table: "Experiments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExperimentPreprocesses",
                table: "ExperimentPreprocesses");

            migrationBuilder.RenameTable(
                name: "Experiments",
                newName: "Experiment");

            migrationBuilder.RenameTable(
                name: "ExperimentPreprocesses",
                newName: "ExperimentPreprocess");

            migrationBuilder.RenameIndex(
                name: "IX_Experiments_TrainingHistoryId",
                table: "Experiment",
                newName: "IX_Experiment_TrainingHistoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Experiments_TenantId",
                table: "Experiment",
                newName: "IX_Experiment_TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_Experiments_TemplateVersionId",
                table: "Experiment",
                newName: "IX_Experiment_TemplateVersionId");

            migrationBuilder.RenameIndex(
                name: "IX_Experiments_TemplateId",
                table: "Experiment",
                newName: "IX_Experiment_TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_Experiments_ExperimentPreprocessId",
                table: "Experiment",
                newName: "IX_Experiment_ExperimentPreprocessId");

            migrationBuilder.RenameIndex(
                name: "IX_Experiments_DataSetVersionId",
                table: "Experiment",
                newName: "IX_Experiment_DataSetVersionId");

            migrationBuilder.RenameIndex(
                name: "IX_Experiments_DataSetId",
                table: "Experiment",
                newName: "IX_Experiment_DataSetId");

            migrationBuilder.RenameIndex(
                name: "IX_ExperimentPreprocesses_TrainingHistoryId",
                table: "ExperimentPreprocess",
                newName: "IX_ExperimentPreprocess_TrainingHistoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ExperimentPreprocesses_TenantId",
                table: "ExperimentPreprocess",
                newName: "IX_ExperimentPreprocess_TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_ExperimentPreprocesses_TemplateVersionId",
                table: "ExperimentPreprocess",
                newName: "IX_ExperimentPreprocess_TemplateVersionId");

            migrationBuilder.RenameIndex(
                name: "IX_ExperimentPreprocesses_TemplateId",
                table: "ExperimentPreprocess",
                newName: "IX_ExperimentPreprocess_TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_ExperimentPreprocesses_DataSetVersionId",
                table: "ExperimentPreprocess",
                newName: "IX_ExperimentPreprocess_DataSetVersionId");

            migrationBuilder.RenameIndex(
                name: "IX_ExperimentPreprocesses_DataSetId",
                table: "ExperimentPreprocess",
                newName: "IX_ExperimentPreprocess_DataSetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Experiment",
                table: "Experiment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExperimentPreprocess",
                table: "ExperimentPreprocess",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiment_AquariumDatasets_DataSetId",
                table: "Experiment",
                column: "DataSetId",
                principalTable: "AquariumDatasets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Experiment_AquariumDatasetVersions_DataSetVersionId",
                table: "Experiment",
                column: "DataSetVersionId",
                principalTable: "AquariumDatasetVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Experiment_ExperimentPreprocess_ExperimentPreprocessId",
                table: "Experiment",
                column: "ExperimentPreprocessId",
                principalTable: "ExperimentPreprocess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Experiment_Templates_TemplateId",
                table: "Experiment",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Experiment_TemplateVersions_TemplateVersionId",
                table: "Experiment",
                column: "TemplateVersionId",
                principalTable: "TemplateVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Experiment_Tenants_TenantId",
                table: "Experiment",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Experiment_TrainingHistories_TrainingHistoryId",
                table: "Experiment",
                column: "TrainingHistoryId",
                principalTable: "TrainingHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentPreprocess_AquariumDatasets_DataSetId",
                table: "ExperimentPreprocess",
                column: "DataSetId",
                principalTable: "AquariumDatasets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentPreprocess_AquariumDatasetVersions_DataSetVersion~",
                table: "ExperimentPreprocess",
                column: "DataSetVersionId",
                principalTable: "AquariumDatasetVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentPreprocess_Templates_TemplateId",
                table: "ExperimentPreprocess",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentPreprocess_TemplateVersions_TemplateVersionId",
                table: "ExperimentPreprocess",
                column: "TemplateVersionId",
                principalTable: "TemplateVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentPreprocess_Tenants_TenantId",
                table: "ExperimentPreprocess",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentPreprocess_TrainingHistories_TrainingHistoryId",
                table: "ExperimentPreprocess",
                column: "TrainingHistoryId",
                principalTable: "TrainingHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
