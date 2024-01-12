# README

## Projeto

## Meu resumo do que eu to entendendo (da parte sobre o 3D)

*Contém muitos erros e abrevições.*

### Transformando os pontos 3D em 2D:

Temos então:
	$$ [x, y, z] $$
e precisamos transformar em:
	$$ [x, y, z] \to [x, y]$$

#### Normalizando o tamanho da tela:

como tem muitos tamanhos de telas e tipos diferente, é conveninente reduzir para um tamanho normalizado de tela
uma forma seria particionar a tela indo de -1 a +1, verticalmente e horizontalmente
como a altura e a largura tem tamanhos diferentes, precisamos deixalas proporcionais

por isso vamos usar o "aspect ratio" que será nossa proporção dado por altura/largura

$$ [x, y, z] \to \left[ \left({w \over h} \right) x, y, z \right] $$

#### Campo de visão:

para definir o tamanho e o "zoom" no que estamos vendo, vamos ter que tratar o campo de visão como um triangulo (geralmente isoceles) tendo seus lados como as máximas -1 e +1 da tela. O angulo oposto a base, será nosso angulo $Θ$ quando menor o seu campo de visão ($Θ$) mais da tela o objeto ira ocupar, dando a impressão de zoom. O oposto acontece aumentando o tamanho do seu campo de visão, os objetos parecem mais distantes. Uma forma de lidar com o fator de escala (aspect ratio), seria dividirmos esse triangulo em 2 triangulos retangulos. Conforme o angulo de visão ($Θ$) aumenta, nosso cateto oposto também aumenta.

Com isso, conseguimos uma constante q pode ser dada por:

$$ tan\left({Θ \over 2}\right) $$

Porém, com essa equação, quanto maior nosso campo de visão ($Θ$), maior a nossa constante, o que é o oposto do que acontece. Quanto maior o campo de visão, mais espremidas (menores) as coisas ficam na tela, logo precisamos do oposto disso, o qual chamaremos de $f$:

$$ f = {1 \over tan \left( {Θ \over 2} \right)} $$

Tendo assim:

$$ [x, y, z] \to \left[ \left({w \over h} \right) fx, fy, z \right] $$

#### Terminar de comentar essas formulas

$$ [x, y, z] \to \left[ {{\left({w \over h} \right) fx} \over z}, {fy \over z}, z \left(z_{far} \over z_{far} - z_{near}  \right) - \left({z_{far}  z_{near}} \over z_{far} - z_{near}  \right)\right] $$

$$ [x, y, z] \to \left[ {{\left({w \over h} \right) {1 \over tan \left( {Θ \over 2} \right)}x} \over z}, {{1 \over tan \left( {Θ \over 2} \right)}y \over z}, z \left(z_{far} \over z_{far} - z_{near}  \right) - \left({z_{far}  z_{near}} \over z_{far} - z_{near}  \right)\right] $$


Engines 3D exploram 2 conceitos:
1. Geometria dos Vértices
2. As "Normals" perpendiculares ao plano (Retas Imaginárias perpendiculares aos planos)

### Normals

Para conseguir as normals podemos utilizar a função de "Cross Product", ela funciona nos fornecendo um vetor, dado outros 2 vetores

$$ a = 3i + 5j - 7k $$
$$ b = 2i - 6j + 4k $$

$$ a \times b = \begin{bmatrix} x & y & z\\ 3 & 5 & -7\\ 2 & -6 & 4 \end{bmatrix} $$

$$ x \begin{bmatrix} 5 & -7 \\ -6 & 4 \end{bmatrix} - y \begin{bmatrix} 3 & -7 \\ 2 & 4\end{bmatrix} + \begin{bmatrix} 3 & 5 \\ 2 & -6 \end{bmatrix} $$

$$ x(5 \times 4 - -6 \times -7) - y(3 \times 4 - 4 \times -7)... \\ \to \\ -22x - 26y -28z $$

enconctrar determinante
