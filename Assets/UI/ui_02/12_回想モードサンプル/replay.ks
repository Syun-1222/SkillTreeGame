;==============================================================================
; 回想モード　画面作成
;==============================================================================
; scenario フォルダ内に replay.ks を配置
; image フォルダ内に replay_bth13_dk フォルダを配置
; 必要に応じて枚数やページの増減をおこなってください

;------------------------------------------------------------------------------
*start
;------------------------------------------------------------------------------

[mask time="100"]

; 初期化
[layopt layer="message0" visible="false"]
[layopt layer="0" visible="true"]
[hidemenubutton]

[clearfix]
[cm]

[iscript]

	tf.page                = 0   // ページ番号
	tf.selected_replay_obj = ""; // 選択されたリプレイを一時的に保管

	tf.replay_img_path     = "../image/replay_bth13_dk/"; // 画像のパス

	// 未開放サムネ画像
	tf.replay_thumb = {
		img     : tf.replay_img_path + 'lock.png',
		width:  480,
		height: 270,
		posx: [228, 720, 1212],
		posy: [280, 562]
	};

	// 前ページボタン
	tf.btn_prev = {
		img     : tf.replay_img_path + 'btn_prev.png',
		img_hov : tf.replay_img_path + 'btn_prev_hov.png',
		img_clk : tf.replay_img_path + 'btn_prev_clk.png',
		width:   96,
		height: 120,
		posx:    96,
		posy:   498
	};

	// 次ページボタン
	tf.btn_next = {
		img     : tf.replay_img_path + 'btn_next.png',
		img_hov : tf.replay_img_path + 'btn_next_hov.png',
		img_clk : tf.replay_img_path + 'btn_next_clk.png',
		width:   90,
		height: 120,
		posx:  1734,
		posy:   498
	};

	// 回想モード終了ボタン
	tf.replay_close = {
		img     : tf.replay_img_path + 'btn_back.png',
		img_hov : tf.replay_img_path + 'btn_back_hov.png',
		img_clk : tf.replay_img_path + 'btn_back_clk.png',
		posx    : 1756,
		posy    : 56
	};

[endscript]

;-------------------------------------------------------------------------------
; ページネーション（ページ数が変わるときはtextの中身を修正してね）
;-------------------------------------------------------------------------------
[macro name="pagination"]
  [layopt layer="0" visible="true"]
  [free layer="0" name="pagination" time="1"]
  [ptext layer="0" name="pagination" text="&tf.page + 1 + '/3'" x="660" y="900" size="28" color="0xCC7A29" width="600" align="center"]
[endmacro]

;-------------------------------------------------------------------------------
; 背景読込
;-------------------------------------------------------------------------------
[bg storage="&tf.replay_img_path +'bg.png'" time="100" wait="false"]
[image name="label_recollection anim_label_slide_nlp" storage="&tf.replay_img_path + 'label_recollection.png'" layer="0" x="0" y="0" time="100" wait="false"]

[jump target="*replaypage"]

;------------------------------------------------------------------------------
*replaypage
[cm]

; 回想モード終了
[button name="back_btn" graphic="&tf.replay_close.img" enterimg="&tf.replay_close.img_hov" activeimg="&tf.replay_close.img_clk" target="*backtitle" x="&tf.replay_close.posx" y="&tf.replay_close.posy"]

[iscript]
	tf.target_page = "page_"+tf.page;
[endscript]

*replayview

[jump target="&tf.target_page"]

;------------------------------------------------------------------------------
*page_0
;------------------------------------------------------------------------------
; 1ページ目
[replay_image_button name="" graphic="" no_graphic="&tf.replay_thumb.img" x="&tf.replay_thumb.posx[0]" y="&tf.replay_thumb.posy[0]" width="&tf.replay_thumb.width" height="&tf.replay_thumb.height"]
[replay_image_button name="" graphic="" no_graphic="&tf.replay_thumb.img" x="&tf.replay_thumb.posx[1]" y="&tf.replay_thumb.posy[0]" width="&tf.replay_thumb.width" height="&tf.replay_thumb.height"]
[replay_image_button name="" graphic="" no_graphic="&tf.replay_thumb.img" x="&tf.replay_thumb.posx[2]" y="&tf.replay_thumb.posy[0]" width="&tf.replay_thumb.width" height="&tf.replay_thumb.height"]

[replay_image_button name="" graphic="" no_graphic="&tf.replay_thumb.img" x="&tf.replay_thumb.posx[0]" y="&tf.replay_thumb.posy[1]" width="&tf.replay_thumb.width" height="&tf.replay_thumb.height"]
[replay_image_button name="" graphic="" no_graphic="&tf.replay_thumb.img" x="&tf.replay_thumb.posx[1]" y="&tf.replay_thumb.posy[1]" width="&tf.replay_thumb.width" height="&tf.replay_thumb.height"]
[replay_image_button name="" graphic="" no_graphic="&tf.replay_thumb.img" x="&tf.replay_thumb.posx[2]" y="&tf.replay_thumb.posy[1]" width="&tf.replay_thumb.width" height="&tf.replay_thumb.height"]

; 次ページへ
[button name="anim_arrow_right_lp" graphic="&tf.btn_next.img" enterimg="&tf.btn_next.img_hov" activeimg="&tf.btn_next.img_clk" x="&tf.btn_next.posx" y="&tf.btn_next.posy" target="*nextpage"]

; ページネーション
[pagination]

; 共通処理へ
[jump target ="*common"]

;------------------------------------------------------------------------------
*page_1
;------------------------------------------------------------------------------
; 2ページ目
[replay_image_button name="" graphic="" no_graphic="&tf.replay_thumb.img" x="&tf.replay_thumb.posx[0]" y="&tf.replay_thumb.posy[0]" width="&tf.replay_thumb.width" height="&tf.replay_thumb.height"]
[replay_image_button name="" graphic="" no_graphic="&tf.replay_thumb.img" x="&tf.replay_thumb.posx[1]" y="&tf.replay_thumb.posy[0]" width="&tf.replay_thumb.width" height="&tf.replay_thumb.height"]
[replay_image_button name="" graphic="" no_graphic="&tf.replay_thumb.img" x="&tf.replay_thumb.posx[2]" y="&tf.replay_thumb.posy[0]" width="&tf.replay_thumb.width" height="&tf.replay_thumb.height"]

[replay_image_button name="" graphic="" no_graphic="&tf.replay_thumb.img" x="&tf.replay_thumb.posx[0]" y="&tf.replay_thumb.posy[1]" width="&tf.replay_thumb.width" height="&tf.replay_thumb.height"]
[replay_image_button name="" graphic="" no_graphic="&tf.replay_thumb.img" x="&tf.replay_thumb.posx[1]" y="&tf.replay_thumb.posy[1]" width="&tf.replay_thumb.width" height="&tf.replay_thumb.height"]
[replay_image_button name="" graphic="" no_graphic="&tf.replay_thumb.img" x="&tf.replay_thumb.posx[2]" y="&tf.replay_thumb.posy[1]" width="&tf.replay_thumb.width" height="&tf.replay_thumb.height"]

; 前ページへ
[button name="anim_arrow_left_lp"  graphic="&tf.btn_prev.img" enterimg="&tf.btn_prev.img_hov" activeimg="&tf.btn_prev.img_clk" x="&tf.btn_prev.posx" y="&tf.btn_prev.posy" target="*backpage"]
; 次ページへ
[button name="anim_arrow_right_lp" graphic="&tf.btn_next.img" enterimg="&tf.btn_next.img_hov" activeimg="&tf.btn_next.img_clk" x="&tf.btn_next.posx" y="&tf.btn_next.posy" target="*nextpage"]

; ページネーション
[pagination]

; 共通処理へ
[jump target ="*common"]

;------------------------------------------------------------------------------
*page_2
;------------------------------------------------------------------------------
; 3ページ目
[replay_image_button name="" graphic="" no_graphic="&tf.replay_thumb.img" x="&tf.replay_thumb.posx[0]" y="&tf.replay_thumb.posy[0]" width="&tf.replay_thumb.width" height="&tf.replay_thumb.height"]
[replay_image_button name="" graphic="" no_graphic="&tf.replay_thumb.img" x="&tf.replay_thumb.posx[1]" y="&tf.replay_thumb.posy[0]" width="&tf.replay_thumb.width" height="&tf.replay_thumb.height"]
[replay_image_button name="" graphic="" no_graphic="&tf.replay_thumb.img" x="&tf.replay_thumb.posx[2]" y="&tf.replay_thumb.posy[0]" width="&tf.replay_thumb.width" height="&tf.replay_thumb.height"]

[replay_image_button name="" graphic="" no_graphic="&tf.replay_thumb.img" x="&tf.replay_thumb.posx[0]" y="&tf.replay_thumb.posy[1]" width="&tf.replay_thumb.width" height="&tf.replay_thumb.height"]
[replay_image_button name="" graphic="" no_graphic="&tf.replay_thumb.img" x="&tf.replay_thumb.posx[1]" y="&tf.replay_thumb.posy[1]" width="&tf.replay_thumb.width" height="&tf.replay_thumb.height"]
[replay_image_button name="" graphic="" no_graphic="&tf.replay_thumb.img" x="&tf.replay_thumb.posx[2]" y="&tf.replay_thumb.posy[1]" width="&tf.replay_thumb.width" height="&tf.replay_thumb.height"]

; 前ページへ
[button name="anim_arrow_left_lp"  graphic="&tf.btn_prev.img" enterimg="&tf.btn_prev.img_hov" activeimg="&tf.btn_prev.img_clk" x="&tf.btn_prev.posx" y="&tf.btn_prev.posy" target="*backpage"]

; ページネーション
[pagination]

; 共通処理へ
[jump target ="*common"]

;------------------------------------------------------------------------------
*common
;------------------------------------------------------------------------------

[mask_off time="300"]

[s]

;------------------------------------------------------------------------------
; 回想モード終了
;------------------------------------------------------------------------------
*backtitle

[mask time="200"]

[cm]
[freeimage layer="0"]
[freeimage layer="1"]
[freeimage layer="base"]

[iscript]

  tf.system.flag_replay = false; // 回想モード判定解除

[endscript]

[mask_off time="200"]

; ティラノビルダーを使っている場合はstorageの内容をtitle_screen.ksに書き換える
[jump storage="title.ks"]

;------------------------------------------------------------------------------
; 次ページに移動
;------------------------------------------------------------------------------
*nextpage
[emb exp="tf.page++;"]
[jump target="*replaypage"]

;------------------------------------------------------------------------------
; 前ページに移動
;------------------------------------------------------------------------------
*backpage
[emb exp="tf.page--;"]
[jump target="*replaypage"]

;------------------------------------------------------------------------------
; サムネイルをクリックした
;------------------------------------------------------------------------------
*clickcg
[cm]

[iscript]
    tf.system.flag_replay = true; // 回想モードを true に
[endscript]

; ラベルとページネーション削除
[free layer="0" name="label_replay"]
[free layer="0" name="pagination"]

; 回想シナリオへジャンプ
[jump storage="&tf.selected_replay_obj.storage" target="&tf.selected_replay_obj.target"]
[s]

;------------------------------------------------------------------------------
; 未開放のサムネイルをクリックした場合は何もしない
;------------------------------------------------------------------------------
*no_image
[jump  target="*replaypage"]
