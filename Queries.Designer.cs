namespace SOProject
{
    partial class Queries
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gameid = new System.Windows.Forms.TextBox();
            this.PlayerGame = new System.Windows.Forms.RadioButton();
            this.winner = new System.Windows.Forms.RadioButton();
            this.gamesPlayed = new System.Windows.Forms.RadioButton();
            this.query = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.invitation = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.playerName = new System.Windows.Forms.TextBox();
            this.newgame = new System.Windows.Forms.Button();
            this.label_users_connected = new System.Windows.Forms.Label();
            this.mygames = new System.Windows.Forms.RadioButton();
            this.resultofthegame = new System.Windows.Forms.RadioButton();
            this.Playername2 = new System.Windows.Forms.TextBox();
            this.dateQuery = new System.Windows.Forms.RadioButton();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.invite = new System.Windows.Forms.Button();
            this.chat = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gameid
            // 
            this.gameid.Location = new System.Drawing.Point(641, 80);
            this.gameid.Name = "gameid";
            this.gameid.Size = new System.Drawing.Size(122, 22);
            this.gameid.TabIndex = 0;
            this.gameid.Text = "Game ID ";
            this.gameid.TextChanged += new System.EventHandler(this.gameid_TextChanged);
            // 
            // PlayerGame
            // 
            this.PlayerGame.AutoSize = true;
            this.PlayerGame.Location = new System.Drawing.Point(323, 55);
            this.PlayerGame.Name = "PlayerGame";
            this.PlayerGame.Size = new System.Drawing.Size(158, 20);
            this.PlayerGame.TabIndex = 1;
            this.PlayerGame.TabStop = true;
            this.PlayerGame.Text = "Players in each game";
            this.PlayerGame.UseVisualStyleBackColor = true;
            this.PlayerGame.CheckedChanged += new System.EventHandler(this.PlayerGame_CheckedChanged);
            // 
            // winner
            // 
            this.winner.AutoSize = true;
            this.winner.Location = new System.Drawing.Point(323, 81);
            this.winner.Name = "winner";
            this.winner.Size = new System.Drawing.Size(294, 20);
            this.winner.TabIndex = 2;
            this.winner.TabStop = true;
            this.winner.Text = "Winner of a specific game (Provide game ID)";
            this.winner.UseVisualStyleBackColor = true;
            this.winner.CheckedChanged += new System.EventHandler(this.winner_CheckedChanged);
            // 
            // gamesPlayed
            // 
            this.gamesPlayed.AutoSize = true;
            this.gamesPlayed.Location = new System.Drawing.Point(323, 107);
            this.gamesPlayed.Name = "gamesPlayed";
            this.gamesPlayed.Size = new System.Drawing.Size(189, 20);
            this.gamesPlayed.TabIndex = 3;
            this.gamesPlayed.TabStop = true;
            this.gamesPlayed.Text = "Games Played by a Player";
            this.gamesPlayed.UseVisualStyleBackColor = true;
            this.gamesPlayed.CheckedChanged += new System.EventHandler(this.gamesPlayed_CheckedChanged);
            // 
            // query
            // 
            this.query.Location = new System.Drawing.Point(641, 318);
            this.query.Name = "query";
            this.query.Size = new System.Drawing.Size(134, 31);
            this.query.TabIndex = 4;
            this.query.Text = "Send request";
            this.query.UseVisualStyleBackColor = true;
            this.query.Click += new System.EventHandler(this.query_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(363, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Queries:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.invitation,
            this.Column1});
            this.dataGridView1.Location = new System.Drawing.Point(18, 29);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(299, 240);
            this.dataGridView1.TabIndex = 6;
            // 
            // invitation
            // 
            this.invitation.HeaderText = "Invite";
            this.invitation.MinimumWidth = 6;
            this.invitation.Name = "invitation";
            this.invitation.Width = 75;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Connected players";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.Width = 190;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(95, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Connected:";
            // 
            // playerName
            // 
            this.playerName.Location = new System.Drawing.Point(641, 108);
            this.playerName.Name = "playerName";
            this.playerName.Size = new System.Drawing.Size(122, 22);
            this.playerName.TabIndex = 9;
            this.playerName.Text = "Enter player Name";
            this.playerName.TextChanged += new System.EventHandler(this.playerName_TextChanged);
            // 
            // newgame
            // 
            this.newgame.Location = new System.Drawing.Point(57, 415);
            this.newgame.Name = "newgame";
            this.newgame.Size = new System.Drawing.Size(178, 72);
            this.newgame.TabIndex = 10;
            this.newgame.Text = "New Game";
            this.newgame.UseVisualStyleBackColor = true;
            this.newgame.Click += new System.EventHandler(this.newgame_Click);
            // 
            // label_users_connected
            // 
            this.label_users_connected.AutoSize = true;
            this.label_users_connected.Location = new System.Drawing.Point(176, 9);
            this.label_users_connected.Name = "label_users_connected";
            this.label_users_connected.Size = new System.Drawing.Size(14, 16);
            this.label_users_connected.TabIndex = 11;
            this.label_users_connected.Text = "0";
            this.label_users_connected.Click += new System.EventHandler(this.label_users_connected_Click);
            // 
            // mygames
            // 
            this.mygames.AutoSize = true;
            this.mygames.Location = new System.Drawing.Point(323, 133);
            this.mygames.Name = "mygames";
            this.mygames.Size = new System.Drawing.Size(168, 20);
            this.mygames.TabIndex = 13;
            this.mygames.TabStop = true;
            this.mygames.Text = "Players I\'ve played with";
            this.mygames.UseVisualStyleBackColor = true;
            this.mygames.CheckedChanged += new System.EventHandler(this.mygames_CheckedChanged);
            // 
            // resultofthegame
            // 
            this.resultofthegame.AutoSize = true;
            this.resultofthegame.Location = new System.Drawing.Point(323, 159);
            this.resultofthegame.Name = "resultofthegame";
            this.resultofthegame.Size = new System.Drawing.Size(233, 20);
            this.resultofthegame.TabIndex = 14;
            this.resultofthegame.TabStop = true;
            this.resultofthegame.Text = "Game results with a specific player";
            this.resultofthegame.UseVisualStyleBackColor = true;
            this.resultofthegame.CheckedChanged += new System.EventHandler(this.resultofthegame_CheckedChanged);
            // 
            // Playername2
            // 
            this.Playername2.Location = new System.Drawing.Point(641, 157);
            this.Playername2.Name = "Playername2";
            this.Playername2.Size = new System.Drawing.Size(122, 22);
            this.Playername2.TabIndex = 15;
            this.Playername2.Text = "Enter player Name";
            this.Playername2.TextChanged += new System.EventHandler(this.Playername2_TextChanged);
            // 
            // dateQuery
            // 
            this.dateQuery.AutoSize = true;
            this.dateQuery.Location = new System.Drawing.Point(323, 185);
            this.dateQuery.Name = "dateQuery";
            this.dateQuery.Size = new System.Drawing.Size(282, 20);
            this.dateQuery.TabIndex = 16;
            this.dateQuery.TabStop = true;
            this.dateQuery.Text = "Games on the date (please select a range)";
            this.dateQuery.UseVisualStyleBackColor = true;
            this.dateQuery.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(323, 217);
            this.monthCalendar1.MaxDate = new System.DateTime(2025, 1, 31, 0, 0, 0, 0);
            this.monthCalendar1.MinDate = new System.DateTime(2024, 11, 1, 0, 0, 0, 0);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 17;
            this.monthCalendar1.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateChanged);
            // 
            // invite
            // 
            this.invite.Location = new System.Drawing.Point(57, 275);
            this.invite.Name = "invite";
            this.invite.Size = new System.Drawing.Size(193, 32);
            this.invite.TabIndex = 18;
            this.invite.Text = "Invite";
            this.invite.UseVisualStyleBackColor = true;
            this.invite.Click += new System.EventHandler(this.invite_Click);
            // 
            // chat
            // 
            this.chat.Location = new System.Drawing.Point(0, 0);
            this.chat.Name = "chat";
            this.chat.Size = new System.Drawing.Size(75, 23);
            this.chat.TabIndex = 0;
            // 
            // Queries
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(840, 517);
            this.Controls.Add(this.chat);
            this.Controls.Add(this.invite);
            this.Controls.Add(this.monthCalendar1);
            this.Controls.Add(this.dateQuery);
            this.Controls.Add(this.Playername2);
            this.Controls.Add(this.resultofthegame);
            this.Controls.Add(this.mygames);
            this.Controls.Add(this.label_users_connected);
            this.Controls.Add(this.newgame);
            this.Controls.Add(this.playerName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.query);
            this.Controls.Add(this.gamesPlayed);
            this.Controls.Add(this.winner);
            this.Controls.Add(this.PlayerGame);
            this.Controls.Add(this.gameid);
            this.Name = "Queries";
            this.Text = "Queries";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Queries_FormClosing);
            this.Load += new System.EventHandler(this.Queries_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox gameid;
        private System.Windows.Forms.RadioButton PlayerGame;
        private System.Windows.Forms.RadioButton winner;
        private System.Windows.Forms.RadioButton gamesPlayed;
        private System.Windows.Forms.Button query;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox playerName;
        private System.Windows.Forms.Button newgame;
        private System.Windows.Forms.Label label_users_connected;
        private System.Windows.Forms.RadioButton mygames;
        private System.Windows.Forms.RadioButton resultofthegame;
        private System.Windows.Forms.TextBox Playername2;
        private System.Windows.Forms.RadioButton dateQuery;
        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.Button invite;
        private System.Windows.Forms.DataGridViewCheckBoxColumn invitation;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.Button chat;
    }
}