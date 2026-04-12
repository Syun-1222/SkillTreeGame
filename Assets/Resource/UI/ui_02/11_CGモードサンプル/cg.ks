;===============================================================================

; CGモード画面作成

;===============================================================================

; このファイルは scenario フォルダ内に配置、
; image フォルダ内に cgmode_bth13_dk フォルダを配置して使用します
; 必要に応じて枚数やページの増減をおこなってください

;-------------------------------------------------------------------------------
*start
;-------------------------------------------------------------------------------

[mask time="100"]

; 初期化
[layopt layer="message0" visible="false"]
[layopt layer="0" visible="true"]
[layopt layer="1" visible="true"]
[hidemenubutton]

[clearfix]
[cm]

[iscript]

tf.page              = 0   // ページ番号
tf.selected_cg_image = []  // 選択したCGの差分を格納した配列変数
tf.cg_index          = 0   // 上の配列の要素番号

tf.cg_img_path = "../image/cgmode_bth13_dk/"; // 画像のパス

// 未解放サムネ画像
tf.cg_thumb = {
  img: tf.cg_img_path + 'lock.png',
  width:  480,
  height: 270,
  posx: [228, 720, 1212],
  posy: [280, 562]
}

// 前ページボタン
tf.btn_prev = {
  img:     tf.cg_img_path + 'btn_prev.png',
  img_hov: tf.cg_img_path + 'btn_prev_hov.png',
  img_clk: tf.cg_img_path + 'btn_prev_clk.png',
  width:   96,
  height: 120,
  posx:    96,
  posy:   498
}

// 次ページボタン
tf.btn_next = {
  img:     tf.cg_img_path + 'btn_next.png',
  img_hov: tf.cg_img_path + 'btn_next_hov.png',
  img_clk: tf.cg_img_path + 'btn_next_clk.png',
  width:   90,
  height: 120,
  posx:  1734,
  posy:   498
};

// CGモード終了ボタン
tf.gallery_close = {
  img:     tf.cg_img_path + 'btn_back.png',
  img_hov: tf.cg_img_path + 'btn_back_hov.png',
  img_clk: tf.cg_img_path + 'btn_back_clk.png',
  posx:   1756,
  posy:     56
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
; 背景読み込み
;-------------------------------------------------------------------------------
[bg storage="&tf.cg_img_path + 'bg.png'" time="100" wait="false"]

; 左上の見出し（アニメーションさせたくないときは「anim_label_slide_nlp」を削除
[image name="label_gallery anim_label_slide_nlp" storage="&tf.cg_img_path + 'label_gallery.png'" layer="0" x="0" y="0" visible="true" time="350" wait="false"]

[jump target="*cgpage"]

;-------------------------------------------------------------------------------
*cgpage
;-------------------------------------------------------------------------------
[cm]

; ギャラリーモード終了ボタン
[button name="back_btn" graphic="&tf.gallery_close.img" enterimg="&tf.gallery_close.img_hov" activeimg="&tf.gallery_close.img_clk" x="&tf.gallery_close.posx" y="&tf.gallery_close.posy" target="*backtitle"]

; tf.page変数を利用して個別閲覧ボタン作成ラベルにジャンプします
[jump target="& 'page_' + tf.page "]

;-------------------------------------------------------------------------------
*page_0
;-------------------------------------------------------------------------------
; CG閲覧モード画面1ページ目

; graphic には表示する画像のファイル名
; thumb にはサムネイルとして表示する画像のファイル名（記述がなければgraphicのファイルを指定）

; 一段目
;[cg_image_button graphic="room.jpg" thumb="room.jpg" no_graphic="&tf.cg_thumb.img" x="&tf.cg_thumb.posx[0]" y="&tf.cg_thumb.posy[0]" width="&tf.cg_thumb.width" height="&tf.cg_thumb.height"]
[cg_image_button graphic="" thumb="" no_graphic="&tf.cg_thumb.img" x="&tf.cg_thumb.posx[0]" y="&tf.cg_thumb.posy[0]" width="&tf.cg_thumb.width" height="&tf.cg_thumb.height"]
[cg_image_button graphic="" thumb="" no_graphic="&tf.cg_thumb.img" x="&tf.cg_thumb.posx[1]" y="&tf.cg_thumb.posy[0]" width="&tf.cg_thumb.width" height="&tf.cg_thumb.height"]
[cg_image_button graphic="" thumb="" no_graphic="&tf.cg_thumb.img" x="&tf.cg_thumb.posx[2]" y="&tf.cg_thumb.posy[0]" width="&tf.cg_thumb.width" height="&tf.cg_thumb.height"]

; 二段目
[cg_image_button graphic="" thumb="" no_graphic="&tf.cg_thumb.img" x="&tf.cg_thumb.posx[0]" y="&tf.cg_thumb.posy[1]" width="&tf.cg_thumb.width" height="&tf.cg_thumb.height"]
[cg_image_button graphic="" thumb="" no_graphic="&tf.cg_thumb.img" x="&tf.cg_thumb.posx[1]" y="&tf.cg_thumb.posy[1]" width="&tf.cg_thumb.width" height="&tf.cg_thumb.height"]
[cg_image_button graphic="" thumb="" no_graphic="&tf.cg_thumb.img" x="&tf.cg_thumb.posx[2]" y="&tf.cg_thumb.posy[1]" width="&tf.cg_thumb.width" height="&tf.cg_thumb.height"]

; 次ページへ
[button name="anim_arrow_right_lp" graphic="&tf.btn_next.img" target="*nextpage" enterimg="&tf.btn_next.img_hov" activeimg="&tf.btn_next.img_clk" x="&tf.btn_next.posx" y="&tf.btn_next.posy"]

; ページネーション
[pagination]

; 共通処理にジャンプ
[jump target="*common"]

;-------------------------------------------------------------------------------
*page_1
;-------------------------------------------------------------------------------
; CG閲覧モード画面2ページ目

; 一段目
[cg_image_button graphic="" thumb="" no_graphic="&tf.cg_thumb.img" x="&tf.cg_thumb.posx[0]" y="&tf.cg_thumb.posy[0]" width="&tf.cg_thumb.width" height="&tf.cg_thumb.height"]
[cg_image_button graphic="" thumb="" no_graphic="&tf.cg_thumb.img" x="&tf.cg_thumb.posx[1]" y="&tf.cg_thumb.posy[0]" width="&tf.cg_thumb.width" height="&tf.cg_thumb.height"]
[cg_image_button graphic="" thumb="" no_graphic="&tf.cg_thumb.img" x="&tf.cg_thumb.posx[2]" y="&tf.cg_thumb.posy[0]" width="&tf.cg_thumb.width" height="&tf.cg_thumb.height"]

; 二段目
[cg_image_button graphic="" thumb="" no_graphic="&tf.cg_thumb.img" x="&tf.cg_thumb.posx[0]" y="&tf.cg_thumb.posy[1]" width="&tf.cg_thumb.width" height="&tf.cg_thumb.height"]
[cg_image_button graphic="" thumb="" no_graphic="&tf.cg_thumb.img" x="&tf.cg_thumb.posx[1]" y="&tf.cg_thumb.posy[1]" width="&tf.cg_thumb.width" height="&tf.cg_thumb.height"]
[cg_image_button graphic="" thumb="" no_graphic="&tf.cg_thumb.img" x="&tf.cg_thumb.posx[2]" y="&tf.cg_thumb.posy[1]" width="&tf.cg_thumb.width" height="&tf.cg_thumb.height"]

; 前のページへ
[button name="anim_arrow_left_lp" graphic="&tf.btn_prev.img" enterimg="&tf.btn_prev.img_hov" activeimg="&tf.btn_prev.img_clk" target="*backpage" x="&tf.btn_prev.posx" y="&tf.btn_prev.posy"]

; 次のページへ
[button name="anim_arrow_right_lp" graphic="&tf.btn_next.img" enterimg="&tf.btn_next.img_hov" activeimg="&tf.btn_next.img_clk" target="*nextpage" x="&tf.btn_next.posx" y="&tf.btn_next.posy"]

[pagination]

; 共通処理にジャンプ
[jump target="*common"]

;-------------------------------------------------------------------------------
*page_2
;-------------------------------------------------------------------------------
; CG閲覧モード画面3ページ目

; 一段目
[cg_image_button graphic="" thumb="" no_graphic="&tf.cg_thumb.img" x="&tf.cg_thumb.posx[0]" y="&tf.cg_thumb.posy[0]" width="&tf.cg_thumb.width" height="&tf.cg_thumb.height"]
[cg_image_button graphic="" thumb="" no_graphic="&tf.cg_thumb.img" x="&tf.cg_thumb.posx[1]" y="&tf.cg_thumb.posy[0]" width="&tf.cg_thumb.width" height="&tf.cg_thumb.height"]
[cg_image_button graphic="" thumb="" no_graphic="&tf.cg_thumb.img" x="&tf.cg_thumb.posx[2]" y="&tf.cg_thumb.posy[0]" width="&tf.cg_thumb.width" height="&tf.cg_thumb.height"]

; 二段目
[cg_image_button graphic="" thumb="" no_graphic="&tf.cg_thumb.img" x="&tf.cg_thumb.posx[0]" y="&tf.cg_thumb.posy[1]" width="&tf.cg_thumb.width" height="&tf.cg_thumb.height"]
[cg_image_button graphic="" thumb="" no_graphic="&tf.cg_thumb.img" x="&tf.cg_thumb.posx[1]" y="&tf.cg_thumb.posy[1]" width="&tf.cg_thumb.width" height="&tf.cg_thumb.height"]
[cg_image_button graphic="" thumb="" no_graphic="&tf.cg_thumb.img" x="&tf.cg_thumb.posx[2]" y="&tf.cg_thumb.posy[1]" width="&tf.cg_thumb.width" height="&tf.cg_thumb.height"]

; 前のページへ
[button name="anim_arrow_left_lp" graphic="&tf.btn_prev.img" enterimg="&tf.btn_prev.img_hov" activeimg="&tf.btn_prev.img_clk" target="*backpage" x="&tf.btn_prev.posx" y="&tf.btn_prev.posy"]

[pagination]

; 共通処理にジャンプ
[jump target="*common"]

;-------------------------------------------------------------------------------
*common
;-------------------------------------------------------------------------------

[mask_off time="300"]

[s]

;-------------------------------------------------------------------------------
*backtitle
;-------------------------------------------------------------------------------
; タイトルに戻る処理

; 使用したレイヤーをすべて消去
[cm]
[freeimage layer="0"]
[freeimage layer="1"]

; 別のシナリオにジャンプする場合はここを変更
; ティラノビルダーを使っている場合はstorageの内容をtitle_screen.ksに書き換える
[jump storage="title.ks"]

;-------------------------------------------------------------------------------
*nextpage
;-------------------------------------------------------------------------------
; 次のページに移る処理

; 一時変数 tf.page を増加させたうえで *cgpage へ
[eval exp="tf.page++"]
[jump target="*cgpage"]

;-------------------------------------------------------------------------------
*backpage
;-------------------------------------------------------------------------------
; 前のページに移る処理

; 一時変数 tf.page を減少させたうえで *cgpage へ
[eval exp="tf.page--"]
[jump target="*cgpage"]

;-------------------------------------------------------------------------------
*no_image
;-------------------------------------------------------------------------------
; 未解放のCGをクリックしたときの処理
[jump target="*cgpage"]

;-------------------------------------------------------------------------------
*clickcg
;-------------------------------------------------------------------------------
; 解放済みのCGをクリックしたときの処理

; フリーレイヤーとレイヤー１(back)を解放します
[cm]
[freeimage layer="1" page="back"]

; 一時変数 tf.cg_index に 0 をぶち込みます
[eval exp="tf.cg_index = 0"]

;-------------------------------------------------------------------------------
*cg_next_image
;-------------------------------------------------------------------------------
; CGを見ていきます。
; 見るべきCGがひとつしかない場合は、それだけ見たあと *cgpage に戻ります。
; 見るべきCGが複数ある場合(差分がある場合)は、
; 再帰的にこのラベルに飛び直して次のCGを見ていきます。

; 一時変数 tf.storage に表示すべきCGのstorageを代入します
[iscript]
tf.storage = tf.selected_cg_image[tf.cg_index];
[endscript]

; CGを表示してクリックを待ちます。
[freeimage layer="1" page="back"]
[image     layer="1" page="back" storage="&tf.storage" folder="bgimage" width="1920" height="1080"]
[trans     layer="1" time="700"]
[wt]
[l]

; クリックされたら
; 一時変数 tf.cg_index (差分画像がある場合の画像番号)を1増加させます。
[eval exp="tf.cg_index++"]

; まだ表示すべき差分画像が残っているなら、このラベルに飛びなおします。
[if exp=" tf.selected_cg_image.length > tf.cg_index "]
  [jump target="cg_next_image"]

[else]
  [freeimage layer="1" page="back"]
  [freeimage layer="1" page="fore" time="700"]
  [jump target="*cgpage"]

[endif]
